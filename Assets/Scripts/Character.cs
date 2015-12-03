﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {
	CharacterController cc;

	public GameObject projectilePrefab;
    public GameObject gameManager;
    GameManager scoreScript;
    public GameObject spawnManager;
    SpawnManager spawnScript;
	public Transform projectileSpawnPoint, gun;
    public Transform redSpawn, blueSpawn;

    public GameObject pauseMenu, camera;
    FPSInputController moveScript;
    MouseLook mouseScript, mouseCameraScript;

    public Text ammoText, healthText;
    public GameObject reloadText, noAmmoText, doubleText;
    public Slider healthSlider;

    public AudioSource rifleShot;

    public float fireSpeed = 20.0f;
    public bool alive;
    [HideInInspector]
    public string team;
    [HideInInspector]
    public bool paused;

    int framesPerShot = 5, framesSinceLastShot = 0;
    int bulletsInClip, bulletsRemaining, maxAmmo = 150;
    int _playerHealth;
    bool canFire, doublePower;
    string killer;
    int teamScore;

    Rifle rifleScript;

	void Start() {
		//Grab a component and keep a reference to it
		cc = GetComponent<CharacterController> ();
		if (cc == null) {
			Debug.Log ("No CharacterController found.");
		}

        rifleScript = gun.gameObject.GetComponent<Rifle>();

        mouseScript = gameObject.GetComponent<MouseLook>();
        mouseCameraScript = camera.transform.gameObject.GetComponent<MouseLook>();
        moveScript = gameObject.GetComponent<FPSInputController>();
        try {
            if (mouseScript == null || mouseCameraScript == null || moveScript == null) {
                throw new ArgumentNullException("Missing scripts.");
            }
        } catch (ArgumentNullException e) {
            Debug.LogWarning(e.Message);
        }

        pauseMenu.SetActive(false);

        bulletsInClip = 30; bulletsRemaining = maxAmmo;
        playerHealth = 100; canFire = true; alive = true; doublePower = false;
        paused = false;
        healthSlider.value = playerHealth;
        SetAmmoText(); reloadText.SetActive(false); noAmmoText.SetActive(false);

        scoreScript = gameManager.GetComponent<GameManager>();
        spawnScript = spawnManager.GetComponent<SpawnManager>();

        if (gameObject.tag == "Player2") {
            team = "Red";
            teamScore = scoreScript.redScore;
            gameObject.transform.position = redSpawn.position;
        } else {
            team = "Blue";
            teamScore = scoreScript.blueScore;
            gameObject.transform.position = blueSpawn.position;
        }
	}

	void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused) {
            Time.timeScale = 0.0f;
            mouseScript.canLook = false;
            mouseCameraScript.canLook = false;
            moveScript.canMove = false;
            pauseMenu.SetActive(true);
            paused = true;
        }

		//Key Press Stuff
		if (((Input.GetButton("Fire1") && gameObject.tag == "Player") || (Input.GetAxis("Joystick Fire") == 1 && gameObject.tag == "Player2")) 
              && bulletsInClip > 0 && canFire && !paused && alive) {
            if (framesSinceLastShot == framesPerShot) {
                GameObject temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, gun.rotation) as GameObject;
                temp.GetComponent<Projectile>().shooter = gameObject.tag;
                if (doublePower) {
                    temp.GetComponent<Projectile>().projectileDamage *= 2;
                }
                temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * -fireSpeed, ForceMode.Impulse);
                framesSinceLastShot = 0;
                
                rifleShot.Play();
                bulletsInClip--;
            } else {
                framesSinceLastShot++;
            }
		}

        if (bulletsInClip == 0 && bulletsRemaining > 0) {
            reloadText.SetActive(true);
        } else if (bulletsInClip == 0 && bulletsRemaining == 0) {
            noAmmoText.SetActive(true);
        }

        if (Input.GetButton("Reload") && !paused && alive) {
            int refill = 30 - bulletsInClip;

            if (bulletsRemaining > refill) {
                bulletsRemaining -= refill;
                if (bulletsInClip < 30) {
                    rifleScript.reload = true;
                    canFire = false;
                    Invoke("ReloadDone", 1.5f);
                }

                bulletsInClip = 30;
            } else {
                bulletsInClip += bulletsRemaining;
                bulletsRemaining = 0;

                if (bulletsInClip > 0) {
                    rifleScript.reload = true;
                    canFire = false;
                    Invoke("ReloadDone", 1.5f);
                }
            }

            reloadText.SetActive(false);
        }

        SetAmmoText();
        if (doublePower) {
            doubleText.SetActive(true);
        } else {
            doubleText.SetActive(false);
        }
        healthSlider.value = playerHealth;
        healthText.text = playerHealth.ToString();
	}

    void SetAmmoText() {
        ammoText.text = bulletsInClip + " / " + bulletsRemaining;
    }

    void ReloadDone() {
        canFire = true;
    }

    public int playerHealth {
        get { return _playerHealth; }
        set { _playerHealth = value; }
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.CompareTag("Projectile") && alive) {
            killer = c.gameObject.GetComponent<Projectile>().shooter;
            if ((killer == "Player2" && team == "Blue") ||
                (killer == "Player" && team == "Red") ||
                (killer == "Red" && team == "Blue") ||
                (killer == "Blue" && team == "Red") ||
                (killer == "Neutral")) {
                playerHealth -= c.gameObject.GetComponent<Projectile>().projectileDamage;
            }

            if (playerHealth <= 0 && alive) {
                if (killer == "Player") {
                    scoreScript.blueScore += 5;
                } else if (killer == "Player2") {
                    scoreScript.redScore += 5;
                } else if (killer == "Neutral") {
                    teamScore -= 2;
                } else if (killer == "Red") {
                    scoreScript.redScore += 3;
                } else if (killer == "Blue") {
                    scoreScript.blueScore += 3;
                }

                playerHealth = 0;

                alive = false;

                Invoke("Respawn", 3.0f);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Health") && playerHealth != 100) {
            playerHealth += 50;
            if (playerHealth > 100) {
                playerHealth = 100;
            }

            Destroy(other.gameObject, 0.0f);

            if (other.gameObject.transform.position.x < 0) {
                spawnScript.healthAlive[0] = false;
            } else {
                spawnScript.healthAlive[1] = false;
            }
        } else if (other.gameObject.CompareTag("DoublePower") && !doublePower) {
            doublePower = true;
            Invoke("ResetDoublePower", 15f);
            
            Destroy(other.gameObject, 0.0f);
            
            if (other.gameObject.transform.position.x < 0) {
                spawnScript.powerAlive[0] = false;
            } else {
                spawnScript.powerAlive[1] = false;
            }
        } else if (other.gameObject.CompareTag("Ammo") && bulletsRemaining < maxAmmo) {
            bulletsRemaining += 60;
            if (bulletsRemaining > maxAmmo) {
                bulletsRemaining = maxAmmo;
            }
            Destroy(other.gameObject, 0.0f);

            if (other.gameObject.transform.position.x > 0 && other.gameObject.transform.position.z > 0) {
                spawnScript.ammoAlive[0] = false;
            } else if (other.gameObject.transform.position.x > 0 && other.gameObject.transform.position.z < 0) {
                spawnScript.ammoAlive[1] = false;
            } else if (other.gameObject.transform.position.x < 0 && other.gameObject.transform.position.z < 0) {
                spawnScript.ammoAlive[2] = false;
            } else {
                spawnScript.ammoAlive[3] = false;
            }
        } else if (other.gameObject.CompareTag("DeathBlock")) {
            playerHealth = 0;
            teamScore -= 2;
            Invoke("Respawn", 2.0f);
        }
    }

    void ResetDoublePower() {
        doublePower = false;
    }

    void Respawn() {
        bulletsInClip = 30; bulletsRemaining = maxAmmo;
        playerHealth = 100; canFire = true; alive = true; doublePower = false;
        healthSlider.value = playerHealth;
        SetAmmoText(); reloadText.SetActive(false); noAmmoText.SetActive(false);

        if (gameObject.tag == "Player2") {
            gameObject.transform.position = redSpawn.position;
        }  else {
            gameObject.transform.position = blueSpawn.position;
        }
    }
}
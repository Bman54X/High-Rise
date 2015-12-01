using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {
	CharacterController cc;

	public GameObject projectilePrefab;
    public GameObject scoreManager;
    ScoreManager scoreScript;
	public Transform projectileSpawnPoint, gun;

    public Text ammoText;
    public GameObject reloadText, noAmmoText;
    public Slider healthSlider;

    public AudioSource rifleShot;

    public float fireSpeed = 20.0f;
    public bool alive;
    [HideInInspector]
    public string team;

    int framesPerShot = 6, framesSinceLastShot = 0;
    int bulletsInClip, bulletsRemaining;
    int _playerHealth;
    bool canFire;
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

        bulletsInClip = 30; bulletsRemaining = 150;
        playerHealth = 100; canFire = true; alive = true;
        healthSlider.value = playerHealth;
        SetAmmoText(); reloadText.SetActive(false); noAmmoText.SetActive(false);

        scoreScript = scoreManager.GetComponent<ScoreManager>();

        if (gameObject.tag == "Player2") {
            team = "Red";
            teamScore = scoreScript.redScore;
        } else {
            team = "Blue";
            teamScore = scoreScript.blueScore;
        }
	}

	void Update() {
		//Key Press Stuff
		if (Input.GetButton("Fire1") && bulletsInClip > 0 && canFire) {
            if (framesSinceLastShot == framesPerShot) {
                GameObject temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, gun.rotation) as GameObject;
                temp.GetComponent<Projectile>().shooter = gameObject.tag;
                temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * -fireSpeed, ForceMode.Impulse);
                framesSinceLastShot = 0;
                
                rifleShot.Play();
                bulletsInClip--;
                SetAmmoText();
            } else {
                framesSinceLastShot++;
            }
		}

        if (bulletsInClip == 0 && bulletsRemaining > 0) {
            reloadText.SetActive(true);
        } else if (bulletsInClip == 0 && bulletsRemaining == 0) {
            noAmmoText.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
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
            SetAmmoText();
        }

        healthSlider.value = playerHealth;
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
        if (c.gameObject.CompareTag("Projectile")) {
            killer = c.gameObject.GetComponent<Projectile>().shooter;

            if ((killer == "Player2" && team == "Blue") ||
                (killer == "Player" && team == "Red") ||
                (killer == "Red" && team == "Blue") ||
                (killer == "Blue" && team == "Red") ||
                (killer == "Neutral")) {
                playerHealth -= c.gameObject.GetComponent<Projectile>().projectileDamage;
            }

            if (playerHealth <= 0) {
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

                alive = false;
            }
        }
    }

    /*void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Collectible")) {
            Destroy(other.gameObject, 0.0f);
            score += 50;
            //SetScoreText();
        }
    }*/
}
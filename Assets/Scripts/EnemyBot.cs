using UnityEngine;
using System.Collections;

public class EnemyBot : MonoBehaviour {
    // Enumeration to keep track of AI States
    public enum AIState { Idle, Walk, Run, Shoot, Dead };	// Used in State Machine. Easier than numbers.
    public enum Allegiance { Neutral, Red, Blue };

    public Character target1, target2;						// Used to keep track of Robot that EnemyBot is tracking
    Character currentTarget;
    public GameObject gameManager;
    GameManager scoreScript;

    public AudioSource rifleShot;

    float rotationSpeed = 5.0f; 			// Speed robot rotates
    float attackRange = 200.0f;				// Distance robot attacks
    float shootRange = 30.0f; 				// Robot should shoot if withing range

    float dontComeCloserRange = 5.0f; 		// Robot will stop moving at this range and fire

    public Transform hand; 					// SpawnPoint for projectile
    public GameObject projectilePrefab;		// Used to create the prefab
    public float projectileSpeed = 20.0f;	// Used to control projectile firing speed
    
    public Transform[] redSpawns = new Transform[3];
    public Transform[] blueSpawns = new Transform[3];
    public Transform[] neutralSpawns = new Transform[6];

    float nextFire = 0; 					// Used to tell if a bullet should be fired
    float fireRate = 0.15f;			        // Used to slow fire rate of bullet

    Animator anim;							// Used to access animations
    NavMeshAgent navAgent;					// Used to control AI

    AIState state;
    Allegiance allegiance;

    public SkinnedMeshRenderer rend;

    public int _health;
    bool dead = false;

    void Awake() {
        // Player not found
        if (!target1) {
            Debug.Log("Player 1 not found!");
        } else if (!target2) {
            Debug.Log("Player 2 not found!");
        }

        // Grab Animation component and keep a reference to it
        anim = GetComponent<Animator>();

        // No Animator component found
        if (!anim)
            Debug.Log("Animator not found!");

        // Grab NavMeshAgent component and keep a reference to it
        navAgent = GetComponent<NavMeshAgent>();

        // No NavMeshAgent component found
        if (!navAgent)
            Debug.Log("NavMeshAgent not found!");

        scoreScript = gameManager.GetComponent<GameManager>();

        allegiance = Allegiance.Neutral;

        gameObject.transform.position = neutralSpawns[Random.Range(0, 6)].position;
    }

    // Use this for initialization
    void Start() {
        health = 50;

        // Starts Coroutine to locate Player or WayPoints based on distance
        StartCoroutine("startPlaying");
    }


    // Called every 0.3 seconds
    IEnumerator startPlaying() {
        // Infinite loop
        while (!dead) {
            if (allegiance == Allegiance.Red) {
                currentTarget = target1;
            } else if (allegiance == Allegiance.Blue) {
                currentTarget = target2;
            }  else {
                float dist1 = (target1.transform.position - transform.position).magnitude;
                float dist2 = (target2.transform.position - transform.position).magnitude;

                if (dist1 < dist2) {
                    currentTarget = target1;
                }  else {
                    currentTarget = target2;
                }
            }

            if (canSeeTarget()) {
                yield return StartCoroutine("attackPlayer");
            }

            // Wait for next frame update before continuing
            yield return null;
        }

        deadBot();
    }

    void Update() {
        if (allegiance == Allegiance.Red) {
            currentTarget = target1;
        } else if (allegiance == Allegiance.Blue) {
            currentTarget = target2;
        } else {
            float dist1 = (target1.transform.position - transform.position).magnitude;
            float dist2 = (target2.transform.position - transform.position).magnitude;

            if (dist1 < dist2) {
                currentTarget = target1;
            } else {
                currentTarget = target2;
            }
        }
    }

    void createBullet() {
        // Only fire projectile if player is alive, no player, no projectile
        if (currentTarget.alive) {
            // Linked to AnimationEvent instead of time based
            if( Time.time > nextFire )	{// Has enough time passed before a new projectile can be created
                // Create projectile at the Robots hand (spawn point)
                GameObject projectileInstance = Instantiate(projectilePrefab, hand.position, hand.rotation) as GameObject;

                string allegianceName = "";
                if (allegiance == Allegiance.Neutral) {
                    allegianceName = "Neutral";
                } else if (allegiance == Allegiance.Red) {
                    allegianceName = "Red";
                } else if (allegiance == Allegiance.Blue) {
                    allegianceName = "Blue";
                }

                projectileInstance.GetComponent<Projectile>().shooter = allegianceName;

                // Add a constant velocity to projectile
                projectileInstance.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * projectileSpeed);

                // Timestamp projectile firing
                nextFire = Time.time + fireRate;

                rifleShot.Play();
            }
        }
    }

    bool canSeeTarget() {
        // Check if target (the Player) is further than AttackRange
        if (Vector3.Distance(transform.position, currentTarget.transform.position) > attackRange) {
            return false; 	// Player is far, stop function
        }

        // Stores information of what is hit
        RaycastHit hit;

        Debug.DrawLine(transform.position, currentTarget.transform.position, Color.red);

        // Check if there is anything between two points
        if (Physics.Linecast(transform.position, currentTarget.transform.position, out hit)) {
            return true;
            //return hit.transform == currentTarget;			// Did LineCast hit target (the Player)
        } else if (!Physics.Linecast(transform.position, currentTarget.transform.position, out hit)) {// If there is nothing between the Robot and Player
            return true;
        }

        // End function, nothing was hit
        return false;
    }

    IEnumerator attackPlayer() {
        // Save last seen position of Player to keep Robot moving towards it
        Vector3 lastVisiblePlayerPosition = currentTarget.transform.position;

        // Should Robot attack
        while (!dead) {
            // Player doesnt exist
            if (currentTarget == null) {
                yield break;	// Stop
            }

            // Find distance of Player
            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            // Update last position
            lastVisiblePlayerPosition = currentTarget.transform.position;

            // Buffer between player and Robot
            if (distance > dontComeCloserRange && !dead) {
                // Keep moving if there is still room too move forward
                moveTowardsNavMesh(lastVisiblePlayerPosition);
            }

            // Can Robot see player and is Player alive
            if (canSeeTarget() && currentTarget.alive && distance <= shootRange) {
                // Stop running and go back to Idle
                anim.SetFloat("Speed", 0);
                // Stop NavMesh from following
                navAgent.Stop();

                // Play attack animation
                anim.SetBool("Shooting", true);

                // Create bullet can be called here instead of AnimationEvent
                createBullet();
            }

            // Buffer between player and Robot is within stopping range
            if (distance <= dontComeCloserRange && !dead) {
                // Make Robot look at last position Player was seen
                rotateTowards(lastVisiblePlayerPosition);

                // If Player is dead
                if (!currentTarget.alive || dead) {
                    // Stop running and go back to Idle
                    anim.SetFloat("Speed", 0);
                    // Stop NavMesh from following
                    navAgent.Stop();
                }
            } else if (!dead) {
                // Look for player	
                yield return StartCoroutine("searchPlayer", lastVisiblePlayerPosition); // Pass last visible spot of Player to search() 

                // Can still see target, stop looking
                if (!canSeeTarget())
                    yield break;	// stop
            }

            // wait for an update until next check
            yield return null;
        }
    }

    // Look for Player in scene so the Robot can move towards it
    IEnumerator searchPlayer(Vector3 position) { //the position is the last visible player position
        float timeOut = 3.0f; 			// Time before Robot gives up looking

        while (timeOut > 0.0f) {			// Still time to find the Player
            moveTowardsNavMesh(position);	// Keep moving towards last spot Player was seen using NavMeshAgent

            if (canSeeTarget())			// Can Robot still see Player
                yield break; 			// Stop searching for Player, just move towards it

            timeOut -= Time.deltaTime;	// Number of seconds before Robot is bored and stops looking

            yield return null;			// Wait for next frame update to try again
        }
    }

    void rotateTowards(Vector3 position) {
        // Where should Robot look
        Vector3 direction = position - transform.position;
        direction.y = 0;

        if (direction.magnitude < 0.1) //if the direction is less than that value, stop rotate.  Adds a bit of a dead zone.  
            return;

        //LookRotation coverts Vector3 into a Quaternion
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime); //a smooth transition from 1 Quaternion to another

        // Rotate on only the y, gets rid of any x and z rotation
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void moveTowardsNavMesh(Vector3 position) {
        navAgent.Resume();
        navAgent.SetDestination(position);
        anim.SetFloat("Speed", navAgent.speed);
    }

    void deadBot() {
        anim.SetTrigger("Dead");
        anim.SetBool("Shooting", false);
        anim.SetFloat("Speed", 0);
        navAgent.Stop();

        Invoke("Respawn", 3);
    }

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.CompareTag("Projectile")) {
            string killer = c.gameObject.GetComponent<Projectile>().shooter;
            if ((killer == "Player" && allegiance == Allegiance.Red) ||
                (killer == "Player2" && allegiance == Allegiance.Blue) ||
                (allegiance == Allegiance.Neutral && (killer == "Player" || killer == "Player2"))) {
                health -= c.gameObject.GetComponent<Projectile>().projectileDamage;

                if (health <= 0 && !dead) {
                    dead = true;

                    if (allegiance == Allegiance.Neutral) {
                        if (killer == "Player") {
                            scoreScript.blueScore++;
                            allegiance = Allegiance.Blue;
                        } else if (killer == "Player2") {
                            scoreScript.redScore++;
                            allegiance = Allegiance.Red;
                        }
                    } else {
                        allegiance = Allegiance.Neutral;

                        if (killer == "Player") {
                            scoreScript.blueScore += 2;
                        } else if (killer == "Player2") {
                            scoreScript.redScore += 2;
                        }
                    }
                }
            }
        }
    }

    public void Respawn() {
        dead = false;
        anim.SetTrigger("Respawned");
        anim.SetFloat("Speed", 0);
        health = 50;
        navAgent.Resume();

        if (allegiance == Allegiance.Neutral) {
            rend.material.SetColor("_Color", Color.grey);
            gameObject.transform.position = neutralSpawns[Random.Range(0, 6)].position;
        } else if (allegiance == Allegiance.Red) {
            rend.material.SetColor("_Color", Color.red);
            gameObject.transform.position = redSpawns[Random.Range(0, 3)].position;
        } else if (allegiance == Allegiance.Blue) {
            Color color = new Color(0, 0.04f, 0.69f);
            rend.material.SetColor("_Color", color);
            gameObject.transform.position = blueSpawns[Random.Range(0, 3)].position;
        }

        // Starts Coroutine to locate Playerbased on distance
        StartCoroutine("startPlaying");
    }

    public int health {
        get { return _health; }
        set { _health = value; }
    }
}
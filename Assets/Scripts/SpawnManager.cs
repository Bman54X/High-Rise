using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {
    public Rigidbody healthCollectible, doublePowerCollectible, ammoCollectible;
    public Transform[] healthSpawns = new Transform[2];
    public Transform[] doublePowerSpawns = new Transform[2];
    public Transform[] ammoSpawns = new Transform[4];

    [HideInInspector]
    public bool[] healthAlive = new bool[2];
    [HideInInspector]
    public bool[] powerAlive = new bool[2];
    [HideInInspector]
    public bool[] ammoAlive = new bool[4];

	// Use this for initialization
	void Start () {
        //Ensure the prefabs exist
        if (healthCollectible && doublePowerCollectible) {
            //For each spawn point, spawn one of three different objects
            for (int i = 0; i < 2; i++) {
                Instantiate(healthCollectible, healthSpawns[i].position, healthSpawns[i].rotation);
                Instantiate(doublePowerCollectible, doublePowerSpawns[i].position, doublePowerSpawns[i].rotation);
                healthAlive[i] = true;
                powerAlive[i] = true;
            }

            for (int i = 0; i < 4; i++) {
                Instantiate(ammoCollectible, ammoSpawns[i].position, ammoSpawns[i].rotation);
                ammoAlive[i] = true;
            }
        } else {
            Debug.Log("Missing prefab.");
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 2; i++) {
            if (healthAlive[i] == false) {
                StartCoroutine(RespawnHealth(healthSpawns[i]));
                healthAlive[i] = true;
            }
            if (powerAlive[i] == false) {
                StartCoroutine(RespawnPower(healthSpawns[i]));
                powerAlive[i] = true;
            }
        }

        for (int i = 0; i < 4; i++) {
            if (ammoAlive[i] == false) {
                StartCoroutine(RespawnAmmo(ammoSpawns[i]));
                ammoAlive[i] = true;
            }
        }
	}

    IEnumerator RespawnHealth(Transform healthSpawn) {
        yield return new WaitForSeconds(30);

        Instantiate(healthCollectible, healthSpawn.position, healthSpawn.rotation);
    }

    IEnumerator RespawnPower(Transform powerSpawn) {
        yield return new WaitForSeconds(60);

        Instantiate(doublePowerCollectible, powerSpawn.position, powerSpawn.rotation);
    }

    IEnumerator RespawnAmmo(Transform ammoSpawn) {
        yield return new WaitForSeconds(30);

        Instantiate(ammoCollectible, ammoSpawn.position, ammoSpawn.rotation);
    }
}
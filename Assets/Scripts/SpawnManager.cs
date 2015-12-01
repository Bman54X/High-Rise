using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {
    public Rigidbody healthCollectible;
    public Rigidbody doublePowerCollectible;
    public Transform[] healthSpawns = new Transform[2];
    public Transform[] doublePowerSpawns = new Transform[2];

    [HideInInspector]
    public bool[] healthAlive = new bool[2];
    [HideInInspector]
    public bool[] powerAlive = new bool[2];

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
        } else {
            Debug.Log("Missing prefab.");
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
}
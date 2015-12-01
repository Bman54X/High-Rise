using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	float destroyTime = 1.5f;
    string _shooter;
    int _projectileDamage = 5;

	//Destroy the projectile after a certain time
	void Update () {
		Destroy(gameObject, destroyTime);
	}

    void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }

    public string shooter {
        get { return _shooter; }
        set { _shooter = value; }
    }

    public int projectileDamage {
        get { return _projectileDamage; }
        set { _projectileDamage = value; }
    } 
}
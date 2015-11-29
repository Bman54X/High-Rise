using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	float destroyTime = 1.5f;

	//Destroy the projectile after a certain time
	void Update () {
		Destroy(gameObject, destroyTime);
	}

    void OnCollisionEnter(Collision other) {
        /*if (other.gameObject.CompareTag("Object")) {
            Destroy(gameObject);
            Destroy(other.gameObject, 0.0f);
        }*/
    }
}
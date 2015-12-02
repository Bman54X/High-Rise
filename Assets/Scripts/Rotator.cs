using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
    // Update is called once per frame
    void Start() {
        if (gameObject.CompareTag("Ammo")) {
            transform.Rotate(new Vector3(-90, 0, 0));
        }
    }

    void Update() {
        if (gameObject.CompareTag("Ammo")) {
            transform.Rotate(new Vector3(0, 0, 360) * Time.deltaTime);
        } else {
            transform.Rotate(new Vector3(360, 0, 0) * Time.deltaTime);
        }
    }
}
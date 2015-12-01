using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
    // Update is called once per frame
    void Update() {
        transform.Rotate(new Vector3(360, 0, 0) * Time.deltaTime);
    }
}
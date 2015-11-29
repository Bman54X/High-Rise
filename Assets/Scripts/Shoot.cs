using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
    // Rocket Prefab
    public Rigidbody bulletPrefab;
    public Transform projectileSpawnPoint, camera, gun;
    public float fireSpeed = 20.0f;

    // Update is called once per frame
    void Update() {
        // left mouse clicked?
        if (Input.GetButtonDown("Fire1")) {
            if (bulletPrefab) {
                Rigidbody temp = Instantiate(bulletPrefab, projectileSpawnPoint.position, camera.rotation) as Rigidbody;
                temp.AddForce(gun.transform.forward * -fireSpeed, ForceMode.Impulse);
            }
        }
    }
}
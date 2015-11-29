using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {
    public int score;
    //public Text scoreText;

	CharacterController cc;

	public Rigidbody projectilePrefab;
	public Transform projectileSpawnPoint, gun;
	public float fireSpeed = 20.0f;

	void Start () {
		//Grab a component and keep a reference to it
		cc = GetComponent<CharacterController> ();
		if (cc == null) {
			Debug.Log ("No CharacterController found.");
		}

        score = 0;
        //SetScoreText();
	}

	void Update () {
		//Key Press Stuff
		if(Input.GetButton("Fire1")) {
			if (projectilePrefab) {
				Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, gun.rotation) as Rigidbody;
                temp.AddForce(temp.transform.forward * -fireSpeed, ForceMode.Impulse);
                //temp.velocity = transform.TransformDirection(new Vector3(0, 0, fireSpeed));
			} else {
				Debug.Log ("No prefab found.");
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

    /*void SetScoreText() {
        scoreText.text = "Score: " + score.ToString();
    }*/

    /*void OnControllerColliderHit(ControllerColliderHit c) {
        //Debug.Log("Controller hit " + c.gameObject.name);
    }*/
}
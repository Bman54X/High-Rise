using UnityEngine;
using System.Collections;

public class Rifle : MonoBehaviour {
    [HideInInspector]
    public bool reload;

    public AudioSource reloadSound;

	// Use this for initialization
	void Start () {
        reload = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (reload) {
            gameObject.GetComponent<Animation>().Play("rifleReload");
            reloadSound.Play();
            reload = false;
        }
	}
}

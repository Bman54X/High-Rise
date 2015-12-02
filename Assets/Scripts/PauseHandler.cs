using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour {
    //public Toggle muteToggle;
    public GameObject pauseMenu, player, camera;
    MouseLook mouseScript, mouseCameraScript;
    FPSInputController moveScript;

    // Use this for initialization
    void Start() {
        mouseScript = player.transform.gameObject.GetComponent<MouseLook>();
        mouseCameraScript = camera.transform.gameObject.GetComponent<MouseLook>();
        moveScript = player.transform.gameObject.GetComponent<FPSInputController>();
        try {
            if (mouseScript == null || mouseCameraScript == null || moveScript == null) {
                throw new ArgumentNullException("Missing scripts.");
            }
        } catch (ArgumentNullException e) {
            Debug.LogWarning(e.Message);
        }

        //muteToggle.onValueChanged.AddListener(MuteSounds);
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = 1.0f;
            player.transform.gameObject.GetComponent<Character>().paused = false;
            mouseScript.canLook = true;
            mouseCameraScript.canLook = true;
            moveScript.canMove = true;
            pauseMenu.SetActive(false);
        }
    }

    public void ResumeGame() {
        Time.timeScale = 1.0f;
        player.transform.gameObject.GetComponent<Character>().paused = false;
        mouseScript.canLook = true;
        mouseCameraScript.canLook = true;
        moveScript.canMove = true;
        pauseMenu.SetActive(false);
    }

    public void QuitGame() {
        Application.LoadLevel(0);
    }

    /*public void MuteSounds(bool paused) {
        AudioListener.pause = paused;
    }*/
}
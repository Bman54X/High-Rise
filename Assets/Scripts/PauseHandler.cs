using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour {
    public GameObject pauseMenu, player1, camera1, player2, camera2;
    MouseLook mouseScript1, mouseCameraScript1, mouseScript2, mouseCameraScript2;
    FPSInputController moveScript1, moveScript2;

    // Use this for initialization
    void Start() {
        mouseScript1 = player1.transform.gameObject.GetComponent<MouseLook>();
        mouseCameraScript1 = camera1.transform.gameObject.GetComponent<MouseLook>();
        moveScript1 = player1.transform.gameObject.GetComponent<FPSInputController>();

        mouseScript2 = player2.transform.gameObject.GetComponent<MouseLook>();
        mouseCameraScript2 = camera2.transform.gameObject.GetComponent<MouseLook>();
        moveScript2 = player2.transform.gameObject.GetComponent<FPSInputController>();

        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (player1.transform.gameObject.GetComponent<Character>().paused || 
            player2.transform.gameObject.GetComponent<Character>().paused) {
            Time.timeScale = 0.0f;

            player1.transform.gameObject.GetComponent<Character>().paused = true;
            player2.transform.gameObject.GetComponent<Character>().paused = true;

            mouseScript1.canLook = false; mouseScript2.canLook = false;
            mouseCameraScript1.canLook = false; mouseCameraScript2.canLook = false;
            moveScript1.canMove = false; moveScript2.canMove = false;

            pauseMenu.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) {
            Time.timeScale = 1.0f;
            player1.transform.gameObject.GetComponent<Character>().paused = false;
            player2.transform.gameObject.GetComponent<Character>().paused = false;
            mouseScript1.canLook = true; mouseScript2.canLook = true;
            mouseCameraScript1.canLook = true; mouseCameraScript2.canLook = true;
            moveScript1.canMove = true; moveScript2.canMove = true;
            pauseMenu.SetActive(false);
        }
    }

    public void ResumeGame() {
        Time.timeScale = 1.0f;
        player1.transform.gameObject.GetComponent<Character>().paused = false;
        player2.transform.gameObject.GetComponent<Character>().paused = false;
        mouseScript1.canLook = true; mouseScript2.canLook = true;
        mouseCameraScript1.canLook = true; mouseCameraScript2.canLook = true;
        moveScript1.canMove = true; moveScript2.canMove = true;
        pauseMenu.SetActive(false);
    }

    public void QuitGame() {
        Application.LoadLevel(0);
    }
}
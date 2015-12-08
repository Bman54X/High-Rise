using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Text timerLabel;
    public Text winnerText;

    public GameObject gameOver, player1, camera1;
    public GameObject player2, camera2, winnerPanel;
    public GameObject gameOverCamera;
    MouseLook mouseScript1, mouseCameraScript1, mouseScript2, mouseCameraScript2;
    FPSInputController moveScript1, moveScript2;

    float timer = 601;
    bool timerDone = false, enteredOnce = false;

    public Slider redSlider, blueSlider;
    public Text redText, blueText;

    public int redScore, blueScore;
    [HideInInspector]
    public bool winnerDeclared;
    int scoreLimit;
    [HideInInspector]
    string winner;

    // Use this for initialization
    void Start() {
        mouseScript1 = player1.transform.gameObject.GetComponent<MouseLook>();
        mouseCameraScript1 = camera1.transform.gameObject.GetComponent<MouseLook>();
        moveScript1 = player1.transform.gameObject.GetComponent<FPSInputController>();

        mouseScript2 = player2.transform.gameObject.GetComponent<MouseLook>();
        mouseCameraScript2 = camera2.transform.gameObject.GetComponent<MouseLook>();
        moveScript2 = player2.transform.gameObject.GetComponent<FPSInputController>();

        redScore = 0; blueScore = 0;
        winnerDeclared = false;
        scoreLimit = 100;
        gameOverCamera.SetActive(false);
        gameOver.SetActive(false); winnerPanel.SetActive(false);
    }

    void Update() {
        if (!winnerDeclared && !timerDone) {
            if (redScore < 0) {
                redScore = 0;
            } else if (blueScore < 0) {
                blueScore = 0;
            }

            redText.text = redScore.ToString();
            blueText.text = blueScore.ToString();

            redSlider.value = redScore;
            blueSlider.value = blueScore;

            if (redScore >= scoreLimit) {
                redText.text = scoreLimit.ToString();
                redSlider.value = scoreLimit;
                winnerDeclared = true;
                winner = "Red";
            } else if (blueScore >= scoreLimit) {
                blueText.text = scoreLimit.ToString();
                blueSlider.value = scoreLimit;
                winnerDeclared = true;
                winner = "Blue";
            }

            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = Mathf.Floor(timer % 60).ToString("00");

            timerLabel.text = minutes + ":" + seconds;

            if ((minutes == "00" && seconds == "00")) {
                timerDone = true;
                if (blueScore > redScore) {
                    winner = "Blue";
                }
                else if (blueScore < redScore) {
                    winner = "Red";
                }
                else if (blueScore == redScore) {
                    winner = "Tie";
                }
            }
        }

        timer -= Time.deltaTime;

        if ((winnerDeclared || timerDone) && !enteredOnce) {
            Time.timeScale = 0.2f;

            mouseScript1.canLook = false;
            mouseCameraScript1.canLook = false;
            moveScript1.canMove = false;

            mouseScript2.canLook = false;
            mouseCameraScript2.canLook = false;
            moveScript2.canMove = false;

            gameOver.SetActive(true);

            enteredOnce = true;

            Invoke("GameOver", 1.0f);
        }
    }

    void GameOver() {
        gameOver.SetActive(false);
        winnerPanel.SetActive(true); gameOverCamera.SetActive(true);
        player1.SetActive(false); player2.SetActive(false);

        if (timerDone) {
            if (winner != "Tie") {
                winnerText.text = "Time has run out! " + winner + " wins by having the most points!";
            } else {
                winnerText.text = "Time has run out! Alas, teams were tied, so no winner!";
            }
        } else if (winnerDeclared) {
            winnerText.text = winner + " wins by reaching the score limit first!";
        }
    }

    public void ExitGame() {
        Application.Quit();
    }
}
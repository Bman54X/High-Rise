using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Text timerLabel;

    float timer = 601;
    bool timerDone = false;

    void Update() {
        timer -= Time.deltaTime;

        if (!timerDone) {
            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = Mathf.Floor(timer % 60).ToString("00");

            timerLabel.text = minutes + ":" + seconds;

            if ((minutes == "00" && seconds == "00")) {
                timerDone = true;
            }
        }
    }
}

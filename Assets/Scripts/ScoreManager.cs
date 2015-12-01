using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public Slider redSlider, blueSlider;
    public Text redText, blueText;

    public int redScore, blueScore;
    [HideInInspector]
    public bool winnerDeclared;
    int scoreLimit;
    [HideInInspector]
    public string winner;

	// Use this for initialization
	void Start () {
        redScore = 0; blueScore = 0;
        winnerDeclared = false;
        scoreLimit = 100;
	}
	
	// Update is called once per frame
	void Update () {
        if (!winnerDeclared) {
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
        }
	}
}

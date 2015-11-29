using UnityEngine;
using System.Collections;

public class TitleMenuHandler : MonoBehaviour {
    public GameObject titleMenu, options, credits;

	// Use this for initialization
	void Awake() {
        options.SetActive(false);
        credits.SetActive(false);
        titleMenu.SetActive(true);
	}

    public void PlayGame() {
         Application.LoadLevel(1);
    }

    public void OptionsMenu() {
        options.SetActive(true);
        titleMenu.SetActive(false);
    }

    public void CreditsMenu() {
        credits.SetActive(true);
        titleMenu.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
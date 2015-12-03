using UnityEngine;
using System.Collections;

public class TitleMenuHandler : MonoBehaviour {
    public GameObject titleMenu, options, credits;
    public GameObject points, powerups, about, controls;

	// Use this for initialization
	void Awake() {
        about.SetActive(false); options.SetActive(false);
        credits.SetActive(false); titleMenu.SetActive(true);
        points.SetActive(false); powerups.SetActive(false);
        controls.SetActive(false);
	}

    public void PlayGame() {
         Application.LoadLevel(1);
    }

    public void AboutMenu() {
        controls.SetActive(true);
        titleMenu.SetActive(false);
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
using UnityEngine;
using System.Collections;

public class CreditsMenuHandler : MonoBehaviour {
    public GameObject titleMenu, credits;

    public void BackToTitle() {
        credits.SetActive(false);
        titleMenu.SetActive(true);
    }
}
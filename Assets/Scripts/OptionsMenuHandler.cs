using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour {
    public GameObject titleMenu, options;
    //public Toggle muteToggle;

    // Use this for initialization
    void Start() {
      //  muteToggle.onValueChanged.AddListener(MuteSounds);
    }

    public void BackToTitle() {
        options.SetActive(false);
        titleMenu.SetActive(true);
    }

    /*public void MuteSounds(bool paused) {
        AudioListener.pause = paused;
    }*/
}
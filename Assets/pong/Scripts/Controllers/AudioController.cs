using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
    public tk2dUIToggleButton soundsButton;
    public tk2dUIToggleButton musicButton;

	// Use this for initialization
	void Start () {
        if(PlayerPrefs.GetString("sound")=="")
        {
            soundsButton.IsOn = true;
            musicButton.IsOn = true;
            AudioListener.pause = false;
        }
        else
        {
            soundsButton.IsOn = false;
            musicButton.IsOn = false;
            AudioListener.pause = true;
        }

	}

    void switchSounds()
    {
        soundsButton.IsOn = AudioListener.pause;
        musicButton.IsOn = AudioListener.pause;
        AudioListener.pause = !AudioListener.pause;
        if(AudioListener.pause==true)
            PlayerPrefs.SetString("sound","pause");
        else
            PlayerPrefs.SetString("sound", "");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gets the volume set by the user when a scene is loaded. Also returns the game speed to normal speed.
/// </summary>
public class SoundAdjuster : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Time.timeScale = 1; //will allow animation to play when the menu scene is reloaded. Value isn't being changed from GameController for the reasons mentioned there.
        AudioListener.volume = PlayerPrefs.GetFloat("SliderVolumeLevel", AudioListener.volume);
	}
}

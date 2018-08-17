using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reads and saves Audio slider value as well as sets slider's value at start to player's preferred value if it exists.
/// </summary>
public class SliderValue : MonoBehaviour {

    public Slider slider;   /*!< Reference to Audioslider object*/

    void Start()
    {       
        slider.value = PlayerPrefs.GetFloat("SliderVolumeLevel", AudioListener.volume);   //if there are volume preferences use them, otherwise use audioSource.volume
    }

    void Update()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("SliderVolumeLevel", AudioListener.volume);
    }

    /// <summary>
    /// Called when audio slider's value is changed. Saves new volume in PlayerPrefs.
    /// </summary>
    public void SaveSliderValue()
    {
        PlayerPrefs.SetFloat("SliderVolumeLevel", slider.value);
    }
}

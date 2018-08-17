using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls difficulty buttons, reading and setting their states accordingly.
/// Attached to both Easy and Hard buttons.
/// </summary>
public class DifficultySetter : MonoBehaviour {

    void Start()
    {
        if(PlayerPrefs.GetInt("isEasy", 1) == 1) //if easy mode already set, click easy button
        {
            if (gameObject.name == "Easy Button")
            {
                gameObject.GetComponent<Button>().onClick.Invoke();
            }
        } 
        else //if hard mode already set, click hard button
        {
            if (gameObject.name == "Hard Button")
            {
                gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }                
    }
    
    /// <summary>
    /// Called by Hard button in Main Menu onClick.
    /// Sets the difficulty in PlayerPrefs as hard.
    /// </summary>
	public void SetHard()
    {
        PlayerPrefs.SetInt("isEasy", 0);        
    }

    /// <summary>
    /// Called by Easy button in Main Menu onClick.
    /// Sets the difficulty in PlayerPrefs as easy.
    /// </summary>
    public void SetEasy()
    {
        PlayerPrefs.SetInt("isEasy", 1);
    }
}

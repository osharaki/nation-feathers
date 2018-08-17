using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Attached to Start button in Main Menu. 
/// Loads the Basic (i.e. the actual game) scene.
/// </summary>
public class LoadSceneOnClick : MonoBehaviour {

    /// <summary>
    /// Called by Start button in Main Menu onClick.
    /// </summary>
    /// <param name="sceneIndex">Index of scene to be loaded (here 1)</param>
    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}

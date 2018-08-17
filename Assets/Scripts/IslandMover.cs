using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the islands in the Main Menu screen.
/// </summary>
public class IslandMover : MonoBehaviour {

    public float scrollSpeed;   /*!< Island moving speed.*/

    // Update is called once per frame
    void Update () {
        transform.position += new Vector3(1, 0, 1) * scrollSpeed;
        
    }
}

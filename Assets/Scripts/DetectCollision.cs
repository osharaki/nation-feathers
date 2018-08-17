using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deals with collision detection on the collision oracle.
/// </summary>
public class DetectCollision : MonoBehaviour {

    private BirdController birdControllerScript;    
    
    // Use this for initialization
	void Start () {
        birdControllerScript = transform.parent.GetComponent<BirdController>();
    }

    void OnTriggerEnter(Collider other)
    {        
        birdControllerScript.upcomingCollision = true;              
    }

    void OnTriggerExit(Collider other)
    {
        birdControllerScript.upcomingCollision = false;
    }
}

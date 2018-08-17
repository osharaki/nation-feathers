using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sends gameover signal when bird hits a tree
/// </summary>
public class TreeController : MonoBehaviour {
	
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bird")
        {
            GameController.Instance.gameover = true;
        }
    }
}

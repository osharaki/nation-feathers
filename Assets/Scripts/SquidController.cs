using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Making use of functions provided by other classes to be called when a squid collides with the bird. This
/// includes adjusting hunger bar, playing audio and adding score. Also, sends gameover signal if the bird 
/// collides with a leaper squid. 
/// </summary>
public class SquidController : MonoBehaviour {

    public int scoreValue; /*!< Value of the squid.*/

    private GameController gameControllerScript;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
         
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameControllerScript = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Could not find 'GameController' Script!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bird")
        {
            if(tag == "Basic Squid") //A basic squid will be eaten on collision with bird
            {
                GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                gameControllerScript.AddScore(scoreValue);
                HungerLevel.Instance.Replenish();
                audioSource.Play();
            }
            else //A leaper squid will end the game on collision with bird
            {
                GameController.Instance.gameover = true;
            }
        }
        if (other.gameObject.tag == "Water")
        {
            if(tag == "Leaper")
                audioSource.Play();            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains funtions responsible for displaying and controlling hunger levels. Sends gameover signal if hunger levels have been depleted.
/// Creates flashing screen effect using a coroutine when hunger levels are low.
/// </summary>
public class HungerLevel : MonoBehaviour {

    public static HungerLevel Instance;
    public float startingHungerLevel;                             /*!< Initial hunger level.*/
    public float currentHungerLevel;                             /*!< Current hunger level*/
    public Slider hungerSlider;                                 /*!< Reference to the UI's hunger bar.*/
    public Image hungerImage;                                  /*!< Image that flashes on screen when hunger levels are high*/
    public float pulseStep; /*!< How fast the alpha attribute changes on the warning light. The higher the faster.*/
    public float hungerLevelStep;   /*!< How fast the hunger level drops. The higher the faster.*/

    Color hungerImageColor;
    bool coroutineRunning;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        coroutineRunning = false;
        currentHungerLevel = startingHungerLevel;
        hungerImageColor = hungerImage.color;
        if(PlayerPrefs.GetInt("isEasy") == 1)
        {
            hungerLevelStep = 0.05f;
        }
        else
        {
            hungerLevelStep = 0.1f;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameController.Instance.gameover && !GameController.Instance.paused) //if game still running
        {
            if (currentHungerLevel <= 0)
            {
                GameController.Instance.gameover = true;
            }

            if (currentHungerLevel <= 50)
            {
                if (!coroutineRunning)
                {
                    StartCoroutine("ColorPulse", hungerImageColor);
                }
            }
            else
            {
                resetColor();
            }
            currentHungerLevel -= hungerLevelStep;
            hungerSlider.value = currentHungerLevel;
        }
        else //if game is over 
        {
            resetColor();
        }
	}

    /// <summary>
    /// Turns screen color to its normal color.
    /// </summary>
    private void resetColor()
    {
        if (coroutineRunning)
            StopCoroutine("ColorPulse");
        hungerImageColor.a = 0;
        hungerImage.color = hungerImageColor;
        coroutineRunning = false;
    }

    /// <summary>
    /// Responsible for creating the flashing red screen when hunger levels are low.
    /// </summary>
    /// <param name="c">Color component of the hungerImage object</param>
    private IEnumerator ColorPulse(Color c)
    {
        coroutineRunning = true;
        bool incrementing = true;

        while (true)
        {
            //elapsedTime = 0;
            if (incrementing)
            {
                while (c.a < 0.5f)
                {
                    c.a += pulseStep;
                    hungerImage.color = c;
                    yield return null;
                }
                incrementing = false;
            }
            else
            {
                while (c.a > 0)
                {
                    c.a -= pulseStep;
                    hungerImage.color = c;
                    yield return null;
                }
                incrementing = true;
            }
            yield return null;
        }                    
    }

    /// <summary>
    /// Refills hungerBar when food is eaten. Called by SquidController when a squid is eaten.
    /// </summary>
    public void Replenish()
    {
        currentHungerLevel += 10;
        if (currentHungerLevel > 100)
        {
            currentHungerLevel = 100;
        }        
    }
}

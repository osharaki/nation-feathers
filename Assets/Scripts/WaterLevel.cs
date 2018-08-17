using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls water level drop. Using a coroutine.
/// </summary>
public class WaterLevel : MonoBehaviour {

    public static WaterLevel Instance;  /*!< Static reference to this class.*/
    public float sinkRate;  /*!< How fast the water level sinks. The larger this number the faster the rate.*/

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine("Rise");
        if (PlayerPrefs.GetInt("isEasy") == 1)
        {
            sinkRate = 0.00006f;
        }
        else
        {
            sinkRate = 0.0001f;
        }
    }

    IEnumerator Rise()
    {
        float f;
        for (f = 3f; f >= 2.5f; f -= sinkRate)
        {
            while (GameController.Instance.paused || GameController.Instance.gameover)
            {
                yield return new WaitForSeconds(0.2f);
            }
            transform.position = new Vector3(transform.position.x, f, transform.position.z);
            yield return null;
        }
        f = 2.5f;
        transform.position = new Vector3(transform.position.x, f, transform.position.z);
    }
}

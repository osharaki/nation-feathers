using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the foodPool which contains all squid in the game.
/// </summary>
public class FoodPool : MonoBehaviour {
    
    public GameObject[] foodPrefabs;    /*!< Reference to squid prefabs (Leaper and basic).*/

    static int totalFood = 40;  //number of basic squids
    static int totalLeaper = 30;    //number of leaper-squids
    static GameObject[] food;   //Leaper squids start at element=totalFood

    // Use this for initialization
    void Start()
    {
        food = new GameObject[totalFood + totalLeaper];
        for (int i = 0; i < totalFood; i++)
        {
            food[i] = Instantiate(foodPrefabs[0], Vector3.zero, Quaternion.identity);
            food[i].GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }

        for (int i = totalFood; i < totalFood + totalLeaper; i++)
        {
            food[i] = Instantiate(foodPrefabs[1], Vector3.zero, Quaternion.identity);
            food[i].GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
    }

    /// <summary>
    /// Smart planes call this function to get squid from the foodPool. 
    /// </summary>
    /// <returns>Returns normal squid if water levels are high. Both normal and leaper squid if water levels are low, with a 30%
    /// chance of returning leaper squid. Returns null if there are no squid available in the foodPool.</returns>
    static public GameObject getFood()
    {
        if (WaterLevel.Instance.transform.position.y > 2.5f) //give basic squids only
        {
            for (int i = 0; i < totalFood; i++)
            {
                if (!food[i].GetComponentInChildren<SkinnedMeshRenderer>().enabled)
                {
                    return food[i];
                }
            }
        }
        else //give basic AND leaper squids (choose randomly)
        {            
            if(Random.Range(0, 100) > 30) //70% chance of creating basic squid
            {
                for (int i = 0; i < totalFood; i++)
                {
                    if (!food[i].GetComponentInChildren<SkinnedMeshRenderer>().enabled)
                    {
                        return food[i];
                    }
                }
                //if all basic squids are used up try the leaper squids
                for (int i = totalFood; i < totalFood + totalLeaper; i++)
                {
                    if (!food[i].GetComponentInChildren<SkinnedMeshRenderer>().enabled)
                    {
                        return food[i];
                    }
                }
            }
            else //30% chance of creating leaper squids
            {
                for (int i = totalFood; i < totalFood + totalLeaper; i++)
                {
                    if (!food[i].GetComponentInChildren<SkinnedMeshRenderer>().enabled)
                    {
                        return food[i];
                    }
                }
                //if no leaper squids are available we'll have to settle for basic squids
                for (int i = 0; i < totalFood; i++)
                {
                    if (!food[i].GetComponentInChildren<SkinnedMeshRenderer>().enabled)
                    {
                        return food[i];
                    }
                }
            }
        }
        return null; //if no squids at all are available then return null
    }
}

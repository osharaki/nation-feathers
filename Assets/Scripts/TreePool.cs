using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the treePool which contains all trees in the game.
/// </summary>
public class TreePool : MonoBehaviour {

    static int totalTrees = 350;    //total number of trees in the pool
    public GameObject[] treePrefabs;    /*!< This array contains the different tree objects to be chosen from randomly for the pool.*/
    static GameObject[] trees;

	// Use this for initialization
	void Start () {
        trees = new GameObject[totalTrees];
        for(int i = 0; i < totalTrees; i++)
        {
            trees[i] = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length)], Vector3.zero, Quaternion.identity);
            trees[i].GetComponent<Renderer>().enabled = false;
        }
	}
	
    /// <summary>
    /// Smart planes can call this function to receive a tree.
    /// </summary>
    /// <returns>Returns a tree object. Returns null if there are no available trees in the treePool.</returns>
	static public GameObject getTree()
    {
        for(int i = 0; i < totalTrees; i++)
        {
            if (!trees[i].GetComponent<Renderer>().enabled)
            {
                return trees[i];
            }
        }
        return null;
    }
}

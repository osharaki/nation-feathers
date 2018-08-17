using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Represents a smart panel and used as a reference to them when they are initialized in GenerateInfinite.
/// </summary>
class Tile
{
    public GameObject tile;

    public Tile(GameObject t)
    {
        tile = t;
    }
}

/// <summary>
/// Generates and destroys Smart Planes to form the terrain, moves them and controls their speed. 
/// </summary>
public class GenerateInfinite : MonoBehaviour {

    public GameObject plane;    /*!< Reference to Smart Plane prefab.*/
    public GameObject bird; /*!< Reference to bird game object.*/
    public int platformWidth;   /*!< Width (in Smart Planes) of the generated field.*/
    public int platformHeight;  /*!< Height (in Smart Planes) of generated field.*/

    private float speed, acceleratingSpeed, speedHolder;
    private BirdController birdScript;
    private int planeSize = 10;
    private Vector3 startPos;
    private List<Tile> tiles = new List<Tile>();
    private Vector3 pos;

    bool oldSpeedValid = true;
    int iterator = 0;
    int rowsIterator = 0;
    float waterSpeed, waterAccelerate;

    // Use this for initialization
    void Start () {

        transform.position = Vector3.zero;
        startPos = Vector3.zero;

        if (PlayerPrefs.GetInt("isEasy") == 1)
        {
            speed = -0.01f;
            acceleratingSpeed = -0.05f;
            waterAccelerate = 1;
            waterSpeed = 0.2f;
        }
        else
        {
            speed = -0.05f;
            acceleratingSpeed = -0.09f;
            waterAccelerate = 1.8f;
            waterSpeed = 1f;
        }
        speedHolder = speed;

        if (bird != null)
        {
            birdScript = bird.GetComponent<BirdController>();
        }
        else
        {
            Debug.Log("Couldn't find script 'birdController'");
        }


        for (int x = 0; x < platformWidth; x++)
        {
            for (int z = 0; z < platformHeight; z++)
            {
                pos = new Vector3((x * planeSize + startPos.x),
                                            0,
                                            (z * planeSize + startPos.z));
                GameObject t = Instantiate(plane, pos, Quaternion.identity);
                
                if (x == (platformWidth - 1) / 2) //If plane is in middle of screen mark it for adding food
                {                    
                    t.GetComponent<GenerateTerrain>().addFood = true;
                }
                Tile tile = new Tile(t);
                tiles.Add(tile); 
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

        int l = 0;
        AdjustSpeed();
        foreach (Tile x in tiles.ToList<Tile>()){

            if(GameController.Instance.paused || GameController.Instance.gameover)
            {
                speed = 0;
            }
            x.tile.transform.position = x.tile.transform.position + Vector3.forward * speed;

            if (x.tile.transform.position.z <= -8)
            {

                Vector3 pos = new Vector3((x.tile.transform.position.x),
                                            0,
                                            (((platformHeight+iterator) * planeSize)));
                rowsIterator++;
                if(rowsIterator % platformWidth == 0)
                {
                    iterator++;
                }
                                
                GameObject t = Instantiate(plane, pos, Quaternion.identity);
                t.GetComponent<GenerateTerrain>().forcedPos = new Vector3(t.transform.position.x, t.transform.position.y, (((platformHeight) * (planeSize)) + x.tile.transform.position.z));
                if (l == (platformWidth - 1) / 2) //If plane is in middle of screen mark it for adding food
                {                    
                    t.GetComponent<GenerateTerrain>().addFood = true;
                }
                Tile tile = new Tile(t);
                tiles.Add(tile);
                tiles.Remove(x);
                GenerateTerrain GTScript = x.tile.GetComponent<GenerateTerrain>();
                if(GTScript != null)
                {
                    for(int i = 0; i < GTScript.myTrees.Count; i++)
                    {
                        GTScript.myTrees[i].transform.parent = null;
                        GTScript.myTrees[i].GetComponent<Renderer>().enabled = false;
                    }
                    for(int i = 0; i < GTScript.myFood.Count; i++)
                    {
                        GTScript.myFood[i].transform.parent = null;
                        GTScript.myFood[i].GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                    }
                }
                else
                {
                    Debug.Log("Could not find 'Generate Terrain' script");
                }                
                Destroy(x.tile);
                l++;
            }            
        }                 
	}

    /// <summary>
    /// If bird is at edge of Z movement, increases terrain speed to simulate faster movement.
    /// </summary>    
    private void AdjustSpeed()
    {
        if (bird.transform.position.z >= birdScript.maxZ && VirtualJoystick.Instance.pressed) //accelerate            
        {
            if (oldSpeedValid)
            {
                speedHolder = speed;
                oldSpeedValid = false;
            }
                        
            speed = acceleratingSpeed;
            AnimatedUVs.Instance.uvAnimationRate = new Vector2(AnimatedUVs.Instance.uvAnimationRate.x,
                                                                waterAccelerate);    
        }
        else //normal speed
        {
            speed = speedHolder;
            AnimatedUVs.Instance.uvAnimationRate = new Vector2(AnimatedUVs.Instance.uvAnimationRate.x,
                                                                waterSpeed);
            oldSpeedValid = true;
        }
    }
}

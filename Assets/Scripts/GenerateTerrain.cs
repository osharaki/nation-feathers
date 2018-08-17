using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityScript.Lang;

/// <summary>
/// Uses Perlin Noise to create the hilly terrain on smart planes. Keeps track of the plane's child objects 
/// such as food and trees.
/// </summary>
public class GenerateTerrain : MonoBehaviour {

    public int heightScale; /*!< How high the terrain will be.*/
    public float detailScale;   /*!< Adjusts smoothness of the terrain.*/
    public Vector3 forcedPos = Vector3.zero;    /*!< Value applied by GenerateInfinite to change the plane's position after it is first initialized.*/
    public float treeDepth; /*!< A value applied to trees to bury them a certain distance in the plane in order to cover their base.*/
    public int treesPerPlane;   /*!< Amount of trees per plane.*/
    public int foodPerPlane;    /*!< Amount of squid per plane.*/
    public List<GameObject> myTrees = new List<GameObject>(); /*!< List of trees situated on plane.*/
    public List<GameObject> myFood = new List<GameObject>(); /*!< List of squid situated on plane.*/
    public bool addFood = false;    /*!< True if the plane is situated in the middle of the field, in other words where the player can actually go. If true, food (i,e, squid) will be added to the plane.*/
    public GameObject water;    /*!< Reference to water object.*/

    // Use this for initialization
    void Start () {
        water = GameObject.FindWithTag("Water");
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3 treePos = Vector3.zero;
        float max = 0;
                
        for (int v = 0; v < vertices.Length; v++)
        {
            vertices[v].y = Mathf.PerlinNoise((vertices[v].x + transform.position.x) / detailScale,
                                (vertices[v].z + transform.position.z) / detailScale) * heightScale;
            if(max < vertices[v].y)
            {
                max = vertices[v].y;
            }                                                         
        }
        if(forcedPos != Vector3.zero)
        {
            transform.position = forcedPos;                               
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        gameObject.AddComponent<MeshCollider>();        

        //adding trees
        int treesPlaced = 0;
        Vector3[] sortedVertices = vertices.OrderBy(v => v.y).ToArray<Vector3>(); //sorted from smallest to biggest
        //creating n at highest n vertices in plane: Where n=treesPerPlane  
        for (int i = 0; i < treesPerPlane; i++)
        {
            GameObject tree = TreePool.getTree();
            if (tree != null)
            {
                treePos = new Vector3(sortedVertices[(vertices.Length - 1) - i].x + transform.position.x,
                                    sortedVertices[(vertices.Length - 1) - i].y - (tree.GetComponent<BoxCollider>().bounds.size.y * treeDepth),
                                        sortedVertices[(vertices.Length - 1) - i].z + transform.position.z);
                tree.transform.position = treePos;
                tree.transform.parent = transform;
                tree.GetComponent<Renderer>().enabled = true;
                treesPlaced++;
                myTrees.Add(tree);
            }
        }

        //adding food (only if plane is in middle of screen)
        if (addFood)
        {
            int foodPlaced = 0;

            for (int i = 0; i < foodPerPlane; i++)
            {
                GameObject food = FoodPool.getFood();

                //place food at random position on water
                int threshold = 0;
                int randIndex = Random.Range(threshold, sortedVertices.Length);
                while (sortedVertices[randIndex].y >= water.transform.position.y)
                {
                    randIndex = Random.Range(threshold, sortedVertices.Length); 
                }
                Vector3 foodPos = new Vector3(sortedVertices[randIndex].x + transform.position.x,
                                water.transform.position.y, sortedVertices[randIndex].z + transform.position.z);
                food.transform.position = foodPos;
                food.transform.parent = transform;
                food.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                foodPlaced++;
                myFood.Add(food);
                //place food at deepest positions on water. sortedVertices is a list of vertices in ascending order. 
                /*if (food != null)
                {                    

                    if (water != null)
                    {
                        if (sortedVertices[i].y < water.transform.position.y)
                        {
                            Vector3 foodPos = new Vector3(sortedVertices[i].x + transform.position.x, 
                                water.transform.position.y, sortedVertices[i].z + transform.position.z);
                            food.transform.position = foodPos;
                            food.transform.parent = transform;
                            food.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                            foodPlaced++;
                            myFood.Add(food);
                        }
                    }
                }*/
            }
        }                                
    }

    void Update()
    {
        for (int i = 0; i < myFood.Count; i++)
        {
            if(water != null)
            {
                if (myFood[i].tag == "Basic Squid")
                {
                    myFood[i].transform.position = new Vector3(myFood[i].transform.position.x, water.transform.position.y, myFood[i].transform.position.z);
                }                
            }            
        }
    }

    /// <summary>
    /// Sends gameover signal if bird hits terrain
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bird")
        {
            //Debug.Log("hit");
            GameController.Instance.gameover = true;
        }
    }
}

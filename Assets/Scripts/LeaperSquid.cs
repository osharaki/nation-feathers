using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls Leaper Squid's movement using a coroutine.
/// </summary>
public class LeaperSquid : MonoBehaviour {

    public GameObject water;    /*!< Reference to water object*/
    public float leapWait;  /*!< Amount of time to wait under water before the squid leaps*/
    public bool leaped = false; /*!< Set to true when squid leaps and back to false when squid re-enters the water.*/

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        water = GameObject.FindWithTag("Water");
        StartCoroutine("SquidLeap");
        //transform.position = new Vector3(transform.position.x, water.transform.position.y, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if ((transform.position.y + GetComponentInChildren<SphereCollider>().bounds.size.y) < water.transform.position.y)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
	}

    IEnumerator SquidLeap()
    {
        
        while (true)
        {
            if (leaped == false && GetComponentInChildren<SkinnedMeshRenderer>().enabled)
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.AddForce(0, 700, 0);
                leaped = true;
            }
            else if((transform.position.y + GetComponentInChildren<SphereCollider>().bounds.size.y) < water.transform.position.y)
            {
                leaped = false;
                yield return new WaitForSeconds(leapWait);
            }
            yield return null;
        }                
    }
}

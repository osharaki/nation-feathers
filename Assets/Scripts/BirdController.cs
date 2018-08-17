using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Controls bird movement and speed. 
/// Responisible for playing bird sounds and water splash animation and changing the bird's color.
/// Sends gameover signal on collision with hazards.
/// </summary>
public class BirdController : MonoBehaviour {

    public float speed, maxHeight, maxZ, minZ;
    public bool upcomingCollision, diving, floating;
    public Text text;
    public VirtualJoystick vjs;
    public static BirdController Instance;
    public GameObject waterSplatter;
    public AudioSource moveOnWaterSound, waterSplashIn, squawk;

    private float diveSpeed = -1f;
    private Rigidbody rb;

    GameObject seagullMesh; //used for changing the bird's color
    Color origColor;
    bool squawked = false;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        diving = false;
        floating = false;
        upcomingCollision = false;

        squawk = GetComponents<AudioSource>()[0];
        moveOnWaterSound = GetComponents<AudioSource>()[1];
        waterSplashIn = GetComponents<AudioSource>()[2];
        if (PlayerPrefs.GetInt("isEasy") == 1)
        {
            speed = 10;
        }
        else
        {
            speed = 13;
        }
        rb = GetComponent<Rigidbody>();        

        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "SeagullMesh")
            {
                seagullMesh = child.gameObject;
                origColor = seagullMesh.GetComponent<SkinnedMeshRenderer>().materials[0].color;                
            }
        }
    }

    void Update()
    {
        if (GameController.Instance.gameover)
        {
            if (!squawked)
            {
                squawk.Play();
                squawked = true;
            }
        }
        if (upcomingCollision)
        {
            Color warningColor = Color.red;
            seagullMesh.GetComponent<SkinnedMeshRenderer>().materials[0].color = warningColor;
        }
        else
        {
            seagullMesh.GetComponent<SkinnedMeshRenderer>().materials[0].color = origColor;
        }

        if(transform.position.z >= maxZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxZ);
        }
        if (transform.position.z <= minZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minZ);
        }
    }
    
    void FixedUpdate () {

        //Implementing the joystick
        //Vector2 js = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")) * speed;
        Vector2 js = new Vector2(vjs.Horizontal(), vjs.Vertical());
        //diving = CrossPlatformInputManager.GetButton("Dive");
        rb.AddForce(js.x * speed, 0, js.y * speed);
        //rb.velocity = new Vector3(transform.position.x, 0, transform.position.z);
        if (diving)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            //rb.position = new Vector3(transform.position.x, transform.position.y + moveDepth, transform.position.z);            
            rb.velocity = new Vector3(rb.velocity.x, diveSpeed, rb.velocity.z);
        }
        else if (floating)
        {
            //rb.position = new Vector3(transform.position.x, transform.position.y - moveDepth, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            rb.velocity = new Vector3(rb.velocity.x, -1 * diveSpeed, rb.velocity.z);
        }
        else
        {
            //rb.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }  
        
        //setting waterSplatter apparent size by setting its lifetime according to bird's speed (hasn't been implemented)     
    }

    public void GoingDown()
    {
        floating = false;
        diving = true;
    }
    public void GoingUp()
    {
        diving = false;
        floating = true;
    }

    public void Stationary()
    {
        floating = false;
        diving = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Smart Plane")
        {
            GameController.Instance.gameover = true;
        }        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            waterSplatter.SetActive(true);
            moveOnWaterSound.Play();
            waterSplashIn.Play();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            waterSplatter.SetActive(false);
            moveOnWaterSound.Stop();
        }
    }
}

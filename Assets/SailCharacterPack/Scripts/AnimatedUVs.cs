using UnityEngine;
using System.Collections;

public class AnimatedUVs : MonoBehaviour 
{
	public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f );

    public static AnimatedUVs Instance;


    void Awake()
    {
        Instance = this;
    }
    Vector2 uvOffset = Vector2.zero;

    /*void Update()
    {
        StartCoroutine("Rise");
    }*/
    void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
            GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( "_MainTex", uvOffset );
		}
        //StartCoroutine("Rise");
    }

    /*IEnumerator Rise()
    {
        for (float f = 1.7f; f <= 3.0f; f += 0.001f)
        {
            transform.position = new Vector3(transform.position.x, f, transform.position.z);
            yield return null;
        }
    }*/
}
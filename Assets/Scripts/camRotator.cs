using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for controlling camera animation using a coroutine.
/// </summary>
public class camRotator : MonoBehaviour {

    public GameObject bird; /*!< Reference to bird object.*/
    public Animator animator;   /*!< Reference to camera's Animator object.*/
    public float enterDiveDuration; /*!< Time it takes for animation to complete.*/
    public float exitDiveDuration;  /*!< Time it takes for animation to complete.*/
    public AnimationCurve enterDiveCurve;   /*!< Animation curve. How animation will progress over time.*/
    public AnimationCurve exitDiveCurve;    /*!< Animation curve. How animation will progress over time.*/
    public float turnPoint; /*!< Bird y-position where animation will start.*/

    private BirdController birdScript;
    private int diveParameter = Animator.StringToHash("dive");
    private float diveProgress = 0f;
    private IEnumerator currentBlend;
    private bool lastDivingState, lastFloatingState; //prevents re-entering coroutine if birdScript.diving or birdScript.floating hasn't changed since last frame  
    //private Vector3 offset;

    void Start()
    {
        birdScript = bird.GetComponent<BirdController>();
        lastDivingState = birdScript.diving;
        //offset = transform.position - bird.transform.position;
    }
    // Update is called once per frame
    private void Update () {

        //transform.position = new Vector3(transform.position.x, (bird.transform.position.y + offset.y), transform.position.z);
       
        if ((birdScript.diving != lastDivingState && bird.transform.position.y < turnPoint) 
            || (birdScript.floating != lastFloatingState && bird.transform.position.y < turnPoint))
        {
            if (birdScript.diving)
            {
                if (currentBlend != null)
                {
                    StopCoroutine(currentBlend);
                }
                currentBlend = DiveBlend(1f, enterDiveDuration, enterDiveCurve);
                StartCoroutine(currentBlend);
            }
            else if (birdScript.floating)
            {
                if (currentBlend != null)
                {
                    StopCoroutine(currentBlend);
                }
                currentBlend = DiveBlend(0f, exitDiveDuration, exitDiveCurve);
                StartCoroutine(currentBlend);
            }
            else //holds the bird in place if neither floating nor diving
            {
                if (currentBlend != null)
                {
                    StopCoroutine(currentBlend);
                }
            }
            lastDivingState = birdScript.diving;
            lastFloatingState = birdScript.floating;
        }
        /*if (bird.transform.position.y > turnPoint) //force stops camera float animation if bird is above turn point. Downside is that depending on how far the bird dove, the speed of the camera's float animation may be much lower than that of the dive animation which means the float animation may be stopped before the camera reaches it's original position. This can be solved by using a targetWeight equal to diveProgress + 1 for diving and diveProgress - 1 for floating
        {
            if (currentBlend != null)
            {
                //Debug.Log("stopped coroutine");
                StopCoroutine(currentBlend);
            }
        }*/
        //transform.LookAt(bird.transform);
    }

    private IEnumerator DiveBlend(float targetWeight, float duration, AnimationCurve curve)
    {
        float elapsedTime = 0f;
        float startProgress = diveProgress;

        while (elapsedTime < duration)
        {
            diveProgress = Mathf.Lerp(startProgress, targetWeight, curve.Evaluate(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            animator.SetFloat(diveParameter, diveProgress);
            yield return null;
        }
        diveProgress = targetWeight;
        animator.SetFloat(diveParameter, diveProgress);
    }
}

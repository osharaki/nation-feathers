using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Deals with virtual analog stick movement.
/// </summary>
public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    public static VirtualJoystick Instance; /*!< Static reference to this class*/
    public bool pressed = false;    /*!< Touch state.*/

    private Image bgImage;  //Reference for joystick's background image
    private Image joystickImage; //Reference for joystick's image
    private Vector3 inputVector;    //contains joystick's output   

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bgImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        pressed = true;
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform,
                                                                    ped.position,
                                                                    ped.pressEventCamera,
                                                                    out pos))
        {
            pos.x = pos.x / bgImage.rectTransform.sizeDelta.x;  //turns it into a value between -1 and 0
            pos.y = pos.y / bgImage.rectTransform.sizeDelta.y;  //turns it into a value between -1 and 0

            inputVector = new Vector3((pos.x * 2) + 1, 0, (pos.y * 2) - 1); //creates from pos a value between -1 and 1
            inputVector = inputVector.magnitude > 1.0f ? inputVector.normalized : inputVector; //since we're dealing with a rectangle, clicking a corner returns a value such as -1, 1 for the bottom left corner for example. But since the actual visible image is a circle this shouldn't be possible. Therefore the vector is normalized if it's magnitude exceeds 1
            
            joystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImage.rectTransform.sizeDelta.x / 4),
                                                                        inputVector.z * (bgImage.rectTransform.sizeDelta.y / 4));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        pressed = false;
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }    

    /// <summary>
    /// Is called by BirdController to be translated to horizontal movement.
    /// </summary>
    /// <returns>x-component of the stick's input vector</returns>
    public float Horizontal()
    {
        return inputVector.x;
    }

    /// <summary>
    /// Is called by BirdController to be translated to vertical movement.
    /// </summary>
    /// <returns>y-component of the stick's input vector</returns>
    public float Vertical()
    {        
        return inputVector.z;
    }
}

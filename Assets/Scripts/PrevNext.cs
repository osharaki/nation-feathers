using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Specific to the help menu panel. Deals with displaying the correct images in the help section when the Next and
/// Previous buttons are pressed.
/// </summary>
public class PrevNext : MonoBehaviour {

    public Button prevButt;   /*!< Reference to Previous button object.*/
    public Button nextButt; /*!< Reference to Next button object.*/
    public static PrevNext Instance;    /*!< Static reference to this class.*/
    public Image[] imageArr;    /*!< Array of images in the help menu.*/

    int currentImage; //keeps track of which image is being displayed


    void Awake()
    {
        Instance = this;
    }

    void Start () {
        currentImage = 0;
        prevButt.interactable = false;
    }
	
    /// <summary>
    /// Called by Next button onClick. Enables Previous button.
    /// Displays next image if current image is not already the last one.
    /// If last image has been reached Next button is disabled.
    /// </summary>
	public void ShowNext()
    {
        if (currentImage < (imageArr.Length - 1))
        {
            
            prevButt.interactable = true;   //enable previous button
            imageArr[currentImage].gameObject.SetActive(false);
            currentImage++;
            imageArr[currentImage].gameObject.SetActive(true);
        }
        if (currentImage == (imageArr.Length - 1)) //disable next button
        {
            nextButt.interactable = false;
        }
    }

    /// <summary>
    /// Called by Previous button onClick. Enables Next button.
    /// Displays previous image if current image is not already the first.
    /// If first image has been reached Previous button is disabled.
    /// </summary>
    public void ShowPrev()
    {
        if (currentImage > 0)
        {
            
            nextButt.interactable = true;   //enable next button
            imageArr[currentImage].gameObject.SetActive(false);
            currentImage--;
            imageArr[currentImage].gameObject.SetActive(true);
        }
        if (currentImage == 0) //disable previous button
        {
            prevButt.interactable = false;
        }
    }
}

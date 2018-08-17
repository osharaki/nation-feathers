using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for organizing game flow. Receives gameover signals and halts the game 
/// as well as opens the appropriate menu accordingly. Partly responsible for halting as well as restarting 
/// sounds and animations. Updates score. Contains game flow organizing functions that are called by other classes.
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController Instance;  /*!< Static reference to this class.*/
    public int score = 0;   /*!< Current score.*/
    public Text scoreText;  /*!< Reference to displayed text object*/
    public bool gameover = false;   /*!< Game state.*/
    public bool paused = false; /*!< Game state.*/
    public GameObject bird; /*!< Reference to bird game object.*/
    public GameObject GameoverMenuPanel;    /*!< Reference to gameover menu panel.*/
    public GameObject GameoverScoreLabel;   /*!< Reference to gameover score label.*/

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //when controller starts the menu screen should load
        UpdateScore();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameover)
        {
            GetComponent<AudioSource>().Stop();
            BirdController.Instance.moveOnWaterSound.Stop();
            Time.timeScale = 0;
            GameoverMenuPanel.SetActive(true);
            GameoverScoreLabel.GetComponent<Text>().text = score.ToString();
        }
        else if (paused)
        {
            AudioListener.pause = true;
            BirdController.Instance.moveOnWaterSound.Pause();
            Time.timeScale = 0;
        }
        else
        {
            AudioListener.pause = false;
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Updates score display to current score value.
    /// </summary>
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Called by SquidController to add score when a squid is eaten.
    /// </summary>
    /// <param name="newScoreValue">Value to be added to current score. The amount of points gained by eating a single squid.</param>
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void Unpause()
    {
        paused = false;
    }

    public void Pause()
    {
        if (!gameover)
        {
            paused = true;
        }        
    }

    /// <summary>
    /// Loads main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        //Time.timeScale = 1; //should suffice. But it seems its value gets overridden in update, since the game is still paused, just before the new scene is loaded, which requires me to set it again on start in the new scene. That's my assumption after quick thought.
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Reloads current scene.
    /// </summary>
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

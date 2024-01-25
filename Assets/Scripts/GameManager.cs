using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Boolean to store whether the game is paused
    private bool paused = false;

    // Variables to store the game UI and the pause UI
    public GameObject gameUI;
    public GameObject pauseUI;

    // variable to store the playerController
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // Finds the playerController scripts
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Toggles pausing based on escape button
        if (Input.GetKeyDown(KeyCode.Escape) && !PlayerController.won)
        {
            TogglePause();
        }
    }

    // Toggles the pausing of the game
    public void TogglePause()
    {
        // Pauses the game and changes the UI to the pause UI if the game is not paused
        if(!paused)
        {
            paused = true;
            Time.timeScale = 0;
            gameUI.SetActive(false);
            pauseUI.SetActive(true);
        }
        // Resumes the game and changes the UI to the game UI if the game is paused
        else
        {
            paused = false;
            Time.timeScale = 1;
            gameUI.SetActive(true);
            pauseUI.SetActive(false);
        }
    }

    // Restarts the level
    public void Restart()
    {
        // Resets the won condition
        PlayerController.won = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

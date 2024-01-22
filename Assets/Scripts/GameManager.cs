using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Boolean to store whether the game is paused
    private bool paused = false;

    // Variables to store the game UI and the pause UI
    public GameObject gameUI;
    public GameObject pauseUI;

    // Update is called once per frame
    void Update()
    {
        // Toggles pausing based on escape button
        if (Input.GetKeyDown(KeyCode.Escape))
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
}

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused)
            {
                paused = true;
                Time.timeScale = 0;
                gameUI.SetActive(false);
                pauseUI.SetActive(true);
            }
            else
            {
                paused = false;
                Time.timeScale = 1;
                gameUI.SetActive(true);
                pauseUI.SetActive(false);
            }
        }
    }
}

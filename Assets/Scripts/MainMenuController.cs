using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Starts the game
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}

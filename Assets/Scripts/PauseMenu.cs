using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A script to control the pause menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    /// <summary>
    /// Pause the game and stop time.
    /// </summary>
    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    /// <summary>
    /// resume the game and start time.
    /// </summary>
    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Load the first scene
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }
}

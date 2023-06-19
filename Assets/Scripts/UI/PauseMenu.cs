using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pausedMenu;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _player;

    public static bool _isPaused;

    void Start()
    {
        _pausedMenu.SetActive(false);
        _isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_isPaused)
            {
                ResumeGame();
            }
            else
            {
                ShowCursor();
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        _pausedMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;

        _camera.GetComponentInChildren<CameraControl>().enabled = false;
        _player.GetComponentInChildren<PlayerShooting>().enabled = false;
        _player.GetComponentInChildren<PlayerMovement>().enabled = false;
    }

    public void ResumeGame()
    {
        HideCursor();
        _pausedMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
        
        _camera.GetComponentInChildren<CameraControl>().enabled = true;
        _player.GetComponentInChildren<PlayerShooting>().enabled = true;
        _player.GetComponentInChildren<PlayerMovement>().enabled = true;
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}

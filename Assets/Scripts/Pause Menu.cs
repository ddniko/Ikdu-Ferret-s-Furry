using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool Paused;
    private bool GoOn;
    public Canvas PauseMenuScreen;
    //public GameObject PauseMenuUI;
    //public GameObject SettingsUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Paused)
        {
            PauseMenuScreen.gameObject.SetActive(true);
            Debug.Log("Pause");
            Time.timeScale = 0;
            Paused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Paused || GoOn)
        {
            Debug.Log("Unpause");
            Paused = false;
            GoOn = false;
            Time.timeScale = 1f;
            PauseMenuScreen.gameObject.SetActive(false);
        }

    }

    public void Continue()
    {
        GoOn = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}

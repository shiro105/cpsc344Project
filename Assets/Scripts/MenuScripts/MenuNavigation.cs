﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuNavigation : MonoBehaviour {

    public void LoadTitle()
    {
        SceneManager.LoadScene("title_screen");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Introduction");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

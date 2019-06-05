﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    //public GameObject playAgainButton;
    //public GameObject quitGameButton;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playButton = GameObject.Find("Canvas/PlayAgainButton");
        GameObject quitButton = GameObject.Find("Canvas/QuitGameButton");
        Button play = playButton.GetComponent<Button>();
        Button quit = quitButton.GetComponent<Button>();
        
        play.onClick.AddListener(onPlayClick);
        quit.onClick.AddListener(onQuitClick);
    }

    void onPlayClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void onQuitClick()
    {
        Application.Quit();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles all UI related functions
// TODO extract UI element from the GameController
public class UI : MonoBehaviour {

    [Header("UI Settings")]
    public Text deathCount;
    public Text score;
    public Text highScore;
    public Text controlText; // This displays the key needed to activate something in environment

    private void Start()
    {    
        controlText.text = "";
    }

    void Update()
    {
        UpdateScore();
        deathCount.text = "Sacrifices: " + GameController.deathCount;
    }

    void UpdateScore()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(GameController.score);
        score.text = string.Format("{0:D2}:{1:D2}:{2:D2}", 
            timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

        if (GameController.highScore < GameController.score)
        {
            GameController.highScore = GameController.score;
            highScore.text = "High Score: " + string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }
        else
        {
            TimeSpan highScoreSpan = TimeSpan.FromSeconds(GameController.highScore);
            highScore.text = "High Score: " + string.Format("{0:D2}:{1:D2}:{2:D2}",
            highScoreSpan.Minutes, highScoreSpan.Seconds, highScoreSpan.Milliseconds);
        }
        
    }



    public void UpdateControlText(String newText)
    {
        controlText.text = newText;
    }

    public void ClearControlText()
    {
        controlText.text = "";
    }
}

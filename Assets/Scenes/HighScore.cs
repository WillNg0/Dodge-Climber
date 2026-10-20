using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    static private TextMeshProUGUI _UI_TEXT;
    static private int _SCORE = 0;

    void Awake()
    {
        _UI_TEXT = GetComponent<TextMeshProUGUI>();
        if (PlayerPrefs.HasKey("HighScore"))
        {
            SCORE = PlayerPrefs.GetInt("HighScore");
        }
        else 
        {
            PlayerPrefs.SetInt("HighScore", SCORE);
        }
        UpdateUI();
    }

    static public int SCORE
    {
        get {return _SCORE; }
        private set
        {
            _SCORE = value;
            PlayerPrefs.SetInt("HighScore", value);
            UpdateUI();
        }
    }

    static public void TRY_SET_HIGH_SCORE(int scoreToTry)
    {
        if (scoreToTry <= SCORE) return;
        SCORE = scoreToTry;
    }

    // Update is called once per frame
    static void UpdateUI()
    {
        if (_UI_TEXT != null) 
        {
            _UI_TEXT.text = "High score: " + _SCORE.ToString("#,0");
        }
    }

    [Tooltip("Check this box to reset Highscore in PlayerPrefs")]
    public bool resetHighScoreNow = false;

    void OnDrawGizmos()
    {
        if(resetHighScoreNow)
        {
            resetHighScoreNow = false;
            PlayerPrefs.SetInt("HighScore", 0);
            if (PlayerPrefs.HasKey("HighScore"))
            {
                _SCORE = 0;
                UpdateUI();
            }
            Debug.LogWarning("Playerprefs Highscore reset");
        }
    }
}

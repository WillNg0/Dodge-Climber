using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    static private TextMeshProUGUI _UI_TEXT;
    static private int _SCORE = 0;

    void Awake()
    {   
        
        _UI_TEXT = GetComponent<TextMeshProUGUI>();

        //load saved high score
        if (PlayerPrefs.HasKey("HighScore"))
        {
            SCORE = PlayerPrefs.GetInt("HighScore");
        }
        else 
        {
            //create initial high score if none exist
            PlayerPrefs.SetInt("HighScore", SCORE);
        } 
        UpdateUI(); //display loaded score
    } 

    //used to access and update the high score
    static public int SCORE
    {   
        //returns current high score
        get {return _SCORE; }

        //set high score
        private set
        {
            _SCORE = value;
            PlayerPrefs.SetInt("HighScore", value);
            UpdateUI();
        }
    }

    //check whether new high score can be set if current score is higher than saved high score
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
            _UI_TEXT.text = "High score: " + _SCORE.ToString("#,0"); //display score
        }
    }

    [Tooltip("Check this box to reset Highscore in PlayerPrefs")]
    public bool resetHighScoreNow = false;

    void OnDrawGizmos()
    {

        //resets score when checkbox is checked
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

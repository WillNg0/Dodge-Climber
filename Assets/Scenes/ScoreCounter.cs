using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [Header("Dynamic")]
    public int score = 0;
    private TextMeshProUGUI uiText;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        UpdateUI();        
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }
    // Update is called once per frame
    void UpdateUI()
    {
        if(uiText != null)
        {
            uiText.text = "Score: " + score.ToString("#,0");
        }
    }
}

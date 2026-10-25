using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LiveScript : MonoBehaviour
{
    static private TextMeshProUGUI _UI_TEXT;
    static private int _LIVES = 3;

    void Awake()
    {
        //get TMPro component on the text object
        _UI_TEXT = GetComponent<TextMeshProUGUI>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        //reads initial value of lives at game start
        UpdateUI();
    }

    //tracks player lives across all scripts
    static public int LIVES
    {
        get { return _LIVES; }
        set
        {
            _LIVES = value;
            UpdateUI();
        }
    }

    //set lives with remaining lives
    static public void SET_LIVES(int remainingLives)
    {
        LIVES = remainingLives;
    }

    static private void UpdateUI()
    {
        if(_UI_TEXT != null)
        {
            //display lives as x[number of lives] next to heart image
            _UI_TEXT.text = "x" + _LIVES.ToString();
        }   
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

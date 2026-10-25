using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;

    void Awake()
    {
        Time.timeScale = 1f;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void restart()
    {
        Debug.Log("Restart button was pressed");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene_0_");
    }
}

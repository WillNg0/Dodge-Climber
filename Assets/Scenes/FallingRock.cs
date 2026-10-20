using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private float bottomBound;
    [SerializeField] private int rockHealth = 1;
    private ScoreCounter scoreCounter;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreCounter = scoreGO.GetComponent<ScoreCounter>();
        bottomBound = -15f;
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < bottomBound)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        rockHealth -= dmg;

        if(rockHealth <= 0)
        {
            shatterRock();
        }
    }

    void shatterRock()
    {
        int points = 0;
        if(gameObject.CompareTag("LargeRock"))
        {
            points = 100;
        }
        else if (gameObject.CompareTag("medRock"))
        {
            points = 50;
        }
        else if(gameObject.CompareTag("smallRock"))
        {
            points = 25;
        }

        scoreCounter.AddScore(points);
        HighScore.TRY_SET_HIGH_SCORE(scoreCounter.score);
        Destroy(gameObject);
    }
}

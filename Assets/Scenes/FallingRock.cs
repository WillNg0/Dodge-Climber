using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private float bottomBound;
    [SerializeField] private int rockHealth = 1;
    [SerializeField] private GameObject sizeDownRock;
    [SerializeField] private float splitSpreadForce = 5f;
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
        int shatteredRocks = 0;
        if(gameObject.CompareTag("LargeRock"))
        {
            points = 100;
            shatteredRocks = Random.Range(3, 5);
            
        }
        else if (gameObject.CompareTag("medRock"))
        {
            points = 50;
            shatteredRocks = Random.Range(2,4);
        }
        else if(gameObject.CompareTag("smallRock"))
        {
            points = 25;
        }

        scoreCounter.AddScore(points);
        HighScore.TRY_SET_HIGH_SCORE(scoreCounter.score);

        Rigidbody2D originalRb = GetComponent<Rigidbody2D>();
        Vector2 originalVel = Vector2.zero;

        if (originalRb != null)
        {
            originalVel = originalRb.linearVelocity;
        }

        if(sizeDownRock != null)
        {
            
            for (int i = 0; i < shatteredRocks; i++)
            {
                float horizontalDirection = 0;
                if(i % 2 == 0)
                {
                    horizontalDirection = 1f;
                }
                else
                {
                    horizontalDirection = -1f;
                }

                float angle = (360f / shatteredRocks) * i;
                float radians = angle * Mathf.Deg2Rad;

                Vector2 direction = new Vector2(
                    Mathf.Cos(radians),
                    Mathf.Sin(radians)
                );

                Vector3 spawnPosition = transform.position +
                    (Vector3)(direction * 1.25f);

                GameObject smallPiece = Instantiate(sizeDownRock, spawnPosition, Quaternion.identity);
                Rigidbody2D pieceRb = smallPiece.GetComponent<Rigidbody2D>();

                if(pieceRb != null) 
                {
                    pieceRb.gravityScale = 1f;
                    float downwardVel = Random.Range(-4f, -1f);
                    pieceRb.linearVelocity = new Vector2(horizontalDirection * splitSpreadForce, originalVel.y + downwardVel);
                }
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(gameObject.CompareTag("smallRock"))
        {
            if(coll.gameObject.CompareTag("smallRock"))
            {
                return;
            }
            else
            {
                if (coll.gameObject.CompareTag("medRock") || coll.gameObject.CompareTag("LargeRock"))
                {
                    FallingRock largerRock = coll.gameObject.GetComponent<FallingRock>();
                    largerRock.TakeDamage(1);
                    scoreCounter.AddScore(25);
                    HighScore.TRY_SET_HIGH_SCORE(scoreCounter.score);
                }

                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    float randomX = Random.Range(-7f, 7f);
                    float randomY = Random.Range(2f, 7f);
                    rb.linearVelocity = new Vector2(randomX, randomY);
                }
                Destroy(gameObject, 0.05f);
            }
        }
    }
}

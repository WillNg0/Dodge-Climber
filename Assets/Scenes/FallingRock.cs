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
        
        //destroy rock if it has no more health
        if(rockHealth <= 0)
        {
            shatterRock();
        }
    }

    void shatterRock()
    {
        int points = 0;
        int shatteredRocks = 0;

        //determine points and number of shattered rocks based on rock size
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
        
        //apply point multiplier
        int finalPoints = Mathf.RoundToInt(points * ObbySpawner.POINT_MULTIPLIER);

        //add points and see if it sets new high score
        scoreCounter.AddScore(finalPoints);
        HighScore.TRY_SET_HIGH_SCORE(scoreCounter.score);


        Rigidbody2D originalRb = GetComponent<Rigidbody2D>();
        Vector2 originalVel = Vector2.zero;

        if (originalRb != null)
        {
            originalVel = originalRb.linearVelocity;
        }

        //if rock is medium or large
        if(sizeDownRock != null)
        {
            //spawn amount of shattered rocks   
            for (int i = 0; i < shatteredRocks; i++)
            {
                //randomize direction that shattered rocks spawn
                float horizontalDirection = 0;
                if(i % 2 == 0)
                {
                    horizontalDirection = 1f;
                }
                else
                {
                    horizontalDirection = -1f;
                }
    
                //scatter shattered rocks so they dont spawn in each other
                float angle = (360f / shatteredRocks) * i;
                float radians = angle * Mathf.Deg2Rad;

                Vector2 direction = new Vector2(
                    Mathf.Cos(radians),
                    Mathf.Sin(radians)
                );

                //place each rock outside original rock to avoid collision
                Vector3 spawnPosition = transform.position +
                    (Vector3)(direction * 1.25f);

                //spawn the shattered piece and get the rigidbody2d component
                GameObject smallPiece = Instantiate(sizeDownRock, spawnPosition, Quaternion.identity);
                Rigidbody2D pieceRb = smallPiece.GetComponent<Rigidbody2D>();

                if(pieceRb != null) 
                {
                    //pieceRb.gravityScale = 1f;

                    //push the shattered rocks away
                    float downwardVel = Random.Range(-4f, -1f);
                    pieceRb.linearVelocity = new Vector2(horizontalDirection * splitSpreadForce, originalVel.y + downwardVel);
                }
            }
        }
        Destroy(gameObject); //destroy the larger rock after spawning the shattered rocks
    }

    //shattered rock logic
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(gameObject.CompareTag("smallRock"))
        {
            if(coll.gameObject.CompareTag("smallRock"))
            {
                return; //if a small rock collides with another small rock -> do nothing
            }
            else
            {
                //if small rock collides with a medium or large rock
                if (coll.gameObject.CompareTag("medRock") || coll.gameObject.CompareTag("LargeRock"))
                {
                    FallingRock largerRock = coll.gameObject.GetComponent<FallingRock>();
                    largerRock.TakeDamage(1); //deal 1 damage to the larger rock
                    scoreCounter.AddScore(25); //gain points for small rocks hitting other rocks to avoid not wanting to shatter rocks
                    HighScore.TRY_SET_HIGH_SCORE(scoreCounter.score);
                }
                Destroy(gameObject, 0.05f); //destroy small rock after collision
            }
        }
    }
}

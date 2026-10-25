using UnityEngine;

public class ObbySpawner : MonoBehaviour
{
    [Header("Rock Prefabs")]
    [SerializeField] private GameObject smallRock;
    [SerializeField] private GameObject medRock;
    [SerializeField] private GameObject bigRock;

    [Header("Difficulty Scaling")]
    [SerializeField] private float initialSpawnRate = 2.0f;
    [SerializeField] private float minSpawnRate = 0.3f; 
    //[SerializeField] private float maxSpawnRate = 15f;
    [SerializeField] private float spawnRateDifficultyCurve = 0.35f; //how harder the difficulty gets
    [SerializeField] private float difficultyInterval = 5f; //how often the difficulty increases
    private float nextDifficultyInterval; //keep track of when the difficulty should increase
    public static float POINT_MULTIPLIER = 1;

    [Header("Speed Scaling")]
    [SerializeField] private float initialSpeed = 0.3f;
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float speedDifficultyCurve = 0.4f; //how much faster the rocks fall after each difficulty increase

    private float currSpawnRate;
    private float currSpeed;
    private float nextSpawnTime;
    private float minX, maxX, spawnY; //parameters where rock spawns
        


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        currSpawnRate = initialSpawnRate;
        currSpeed = initialSpeed;
        nextSpawnTime = Time.time + currSpawnRate;
        nextDifficultyInterval = Time.time + difficultyInterval;
        
        //get game screen boundaries 
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        minX = topLeft.x + 0.4f; //so the obstacles dont spawn too off screen
        maxX = topRight.x + 0.4f;
        spawnY = topLeft.y + 0.5f; //spawn just above screen 

        POINT_MULTIPLIER = 1; //reset point multiplier back to 1
    }

    // Update is called once per frame
    void Update()
    {   
        //progressively increase difficulty
        if(Time.time >= nextDifficultyInterval)
        {
            Debug.Log("increasing diff");
            IncreaseDifficulty();
            nextDifficultyInterval += difficultyInterval; 
        }

        //rock drop rate tracker
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomRock();
            nextSpawnTime = Time.time + currSpawnRate;
        }   
    }

    void IncreaseDifficulty()
    {   
        POINT_MULTIPLIER += 0.5f; // increase point multiplier to match difficulty
        if(currSpawnRate > minSpawnRate)
        {
            currSpawnRate -= spawnRateDifficultyCurve * Time.deltaTime; //make rocks spawn faster
        }

        if (currSpeed < maxSpeed)
        {
            currSpeed += speedDifficultyCurve * Time.deltaTime; //make rocks fall faster
        }
    }

    void SpawnRandomRock()
    {
        //get random number: 0 (small rock), 1 (medium rock), 2 (large rock)
        int randIndex = Random.Range(0, 3);
        GameObject selectedSize = null;

        switch(randIndex)
        {
            case 0:
                selectedSize = smallRock;
                break;
            case 1:
                selectedSize = medRock;
                break;
            case 2:
                selectedSize = bigRock;
                break;
        }

        if (selectedSize == null) return;

        //pick random spot in x-axis
        float randomX = Random.Range(minX, maxX);

        //spawn rock at random position
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);
        GameObject spawnedRock = Instantiate(selectedSize, spawnPos, Quaternion.identity);

        //set speed of rock
        Rigidbody2D rb = spawnedRock.GetComponent<Rigidbody2D>();
        if(rb != null) {
            rb.linearVelocity = Vector2.down * currSpeed;
        }
    }
}

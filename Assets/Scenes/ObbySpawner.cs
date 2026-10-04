using UnityEngine;

public class ObbySpawner : MonoBehaviour
{
    [Header("Rock Prefabs")]
    [SerializeField] private GameObject smallRock;
    [SerializeField] private GameObject medRock;
    [SerializeField] private GameObject bigRock;

    [Header("Spawn Rate Scaling")]
    [SerializeField] private float initialSpawnRate = 2.0f;
    [SerializeField] private float minSpawnRate = 0.3f;
    [SerializeField] private float spawnRateDifficultyCurve = 0.05f;

    [Header("Speed Scaling")]
    [SerializeField] private float initialSpeed = 0.3f;
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float speedDifficultyCurve = 0.02f;

    private float currSpawnRate;
    private float currSpeed;
    private float nextSpawnTime;
    private float minX, maxX, spawnY;
        


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        currSpawnRate = initialSpawnRate;
        currSpeed = initialSpeed;
        nextSpawnTime = Time.time + currSpawnRate;
        
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        minX = topLeft.x + 0.8f;
        maxX = topRight.x + 0.8f;
        spawnY = topLeft.y + 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseDifficulty();

        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomRock();
            nextSpawnTime = Time.time + currSpawnRate;
        }   
    }

    void IncreaseDifficulty()
    {
        if(currSpawnRate > minSpawnRate)
        {
            currSpawnRate -= spawnRateDifficultyCurve * Time.deltaTime;
        }

        if (currSpeed < maxSpeed)
        {
            currSpeed += speedDifficultyCurve * Time.deltaTime;
        }
    }

    void SpawnRandomRock()
    {
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

        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        GameObject spawnedRock = Instantiate(selectedSize, spawnPos, Quaternion.identity);
        Rigidbody2D rb = spawnedRock.GetComponent<Rigidbody2D>();
        if(rb != null) {
            rb.linearVelocity = Vector2.down * currSpeed;
        }
    }
}

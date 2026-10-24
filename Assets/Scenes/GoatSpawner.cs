using UnityEngine;

public class GoatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goatPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float minSpawnInterval = 1.5f; 
    [SerializeField] private float difficultyCurve = 0.05f; //how hard the game gets each time

    private float currSpawnInterval;
    private float nextSpawnTime;

    private float screenX;
    private float minSpawnY;
    private float maxSpawnY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set initial times and spawn intervals
        currSpawnInterval = spawnInterval;
        nextSpawnTime = Time.time + currSpawnInterval;

        //get screen boundaries
        screenX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 0.3f; //goat spawns slightly off screen
        minSpawnY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.1f, 0)).y;
        maxSpawnY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.8f, 0)).y;
    }


    // Update is called once per frame
    void Update()
    {
        //gradually increase goat spawn rate
        if(currSpawnInterval > minSpawnInterval)
        {
            currSpawnInterval -= difficultyCurve * Time.deltaTime;
        }

        //check when to spawn goat
        if(Time.time >= nextSpawnTime)
        {
            SpawnGoat();
            nextSpawnTime = Time.time + currSpawnInterval;
        }
    }

    void SpawnGoat()
    {   
        //randomize spawn height
        float randY = Random.Range(minSpawnY, maxSpawnY);
        Quaternion visualRotation; //rotate the goat visual

        //randomly select which side to spawn a goat
        int randSide = Random.Range(0, 2);
        float spawnX;
        if (randSide == 0)
        {
            spawnX = -screenX;
            visualRotation = Quaternion.Euler(12f, 90f, -45f); //rotate goat when spawn on left side 
        }
        else
        {
            spawnX = screenX;
            visualRotation = Quaternion.Euler(12f, 270f, 45f); //rotate goat when spawn on right side 
        }

        Vector3 spawnPosition = new Vector3(spawnX, randY, 0f);

        // Keep the root rotation at zero
        GameObject goat = Instantiate(goatPrefab, spawnPosition, Quaternion.identity);

        // Rotate only the visual model
        Transform goatVisual = goat.transform.Find("GoatVisual");

        if (goatVisual != null)
        {
            goatVisual.localRotation = visualRotation;
        }
    }
}

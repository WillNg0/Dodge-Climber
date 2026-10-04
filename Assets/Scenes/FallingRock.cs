using System.Numerics;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private float bottomBound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
}

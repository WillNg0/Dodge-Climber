using System.Collections;
using UnityEngine;

public class Goat : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private float downwardAngle = -1.5f;

    private Rigidbody2D rb;
    private float screenBoundaryX;
    private float dashDir = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get rigidbody of goat
        rb = GetComponent<Rigidbody2D>();

        //get camera's right edge
        float camWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        //goat can travel fully off screen
        screenBoundaryX = camWidth + 3f;

        //determine dash direction based on which side goat spawns at
        if (transform.position.x < 0)
        {
            dashDir = 1f; //goat on left side, move right
        }
        else
        {
            dashDir = -1f; //goat on right side, move left
        }

        //spawn goat and wait so player can react
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(DashAfterSpawn());
    }

    //goat dashes after spawning and waiting
    private IEnumerator DashAfterSpawn()
    {
        yield return new WaitForSeconds(1f);

        //dash
        rb.linearVelocity = new Vector2(dashDir * dashSpeed, downwardAngle);
    }

    // Update is called once per frame
    void Update()
    {
        //destroy goat when off the screen
        if((dashDir > 0 && transform.position.x > screenBoundaryX) ||
           (dashDir < 0 && transform.position.x < -screenBoundaryX)) {
            Destroy(gameObject);
        }
    }
}

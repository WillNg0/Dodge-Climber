using UnityEngine;

public class AxeProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float rotation = -500f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * speed; //throw axe up with constant velocity
        rb.angularVelocity = rotation; //make axe spin
        Destroy(gameObject, 3f); //destroy axe after leaving screen
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //check if axe hit any rock
        if (coll.CompareTag("LargeRock") ||
            coll.CompareTag("medRock") ||
            coll.CompareTag("smallRock"))
        {
            //get the rock that was hit and make it take damage
            FallingRock rock = coll.GetComponent<FallingRock>();
            if(rock != null)
            {
                rock.TakeDamage(damage); 
            }
            Destroy(gameObject); //destroy axe after collision
        }
    }
    // Update is called once per frame
    void Update()
    {
        //destroy axe once its outside the screen visibly
        Vector3 camBound = Camera.main.WorldToViewportPoint(transform.position);
        if(camBound.x < 0f ||
           camBound.x > 1f ||
           camBound.y < 0f ||
           camBound.y > 1f)
        {
            Destroy(gameObject);
        }
    }
}

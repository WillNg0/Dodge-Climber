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
        rb.angularVelocity = rotation;
        Destroy(gameObject, 3f); //destroy axe after leaving screen
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("LargeRock") ||
            coll.CompareTag("medRock") ||
            coll.CompareTag("smallRock"))
        {
            FallingRock rock = coll.GetComponent<FallingRock>();
            if(rock != null)
            {
                rock.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

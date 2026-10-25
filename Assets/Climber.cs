using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Climber : MonoBehaviour
{   

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Health")]
    private int maxLives = 3;
    private int currLives;
    private float invincibilityDur = 1.5f;
    private bool isInvincible = false;
    private Vector3 startPos;
    private SpriteRenderer spriteRender;

    [Header("Shooting")]
    [SerializeField] private GameObject axePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>(); //get rigidbody2d of climber
        currLives = maxLives;
        startPos = transform.position;
        spriteRender = GetComponent<SpriteRenderer>();

        //match the UI text with the starting lives
        LiveScript.SET_LIVES(currLives);
    }

    // Update is called once per frame
    void Update()
    {
        //gets movement input: wasd or arrow keys
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        //press spacebar to throw axe projectile
        if(Input.GetKeyDown(KeyCode.Space)) ThrowAxe();
    }

    void FixedUpdate()
    {
        //move body based on input and speed
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void ThrowAxe()
    {
        if(axePrefab == null) return;
        Instantiate(axePrefab, transform.position, Quaternion.identity); //throw axe from climber's position
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("collision");
        if(isInvincible) return; 

        //lose a life if climber collides with rock
        GameObject collideWith = coll.gameObject;
        if(collideWith.CompareTag("LargeRock") || 
           collideWith.CompareTag("medRock") ||
           collideWith.CompareTag("smallRock"))
        {
            LoseLife();
            Destroy(collideWith);
        }
    }

    //collision logic for goat (want goat to phase through climber but still take damage)
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (isInvincible) return;

        GameObject collideWith = coll.gameObject;
        if(collideWith.CompareTag("Goat"))
        {
            LoseLife();
        }
    }

    void LoseLife()
    {
        currLives--;

        //update lives
        LiveScript.SET_LIVES(currLives);
        
        //if all lives are lost, restart game from beginning
        if(currLives <= 0)
        {
            SceneManager.LoadScene("Scene_0_");
        }
        else
        {
            //restart player at default spawn position if hit
            transform.position = startPos;
            StartCoroutine(BecomeInvincible()); //become temporarily invincible if hit
        }
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        //get IDs for the climber and obstacle layer
        int climberLayer = LayerMask.NameToLayer("Climber");
        int obbyLayer = LayerMask.NameToLayer("Obstacles");

        //ignore contact between climber and obstacle so climber does not stop rocks when invincible
        Physics2D.IgnoreLayerCollision(climberLayer, obbyLayer, true);

        float currInvincibleDur = 0f;
        
        //flash the climber sprite to indicate climber is invincible
        while(currInvincibleDur < invincibilityDur)
        {
            spriteRender.enabled = !spriteRender.enabled;
            yield return new WaitForSeconds(0.15f);
            currInvincibleDur += 0.15f;
        }

        //return climber to normal
        Physics2D.IgnoreLayerCollision(climberLayer, obbyLayer, false);
        spriteRender.enabled = true;
        isInvincible = false;
    }
}

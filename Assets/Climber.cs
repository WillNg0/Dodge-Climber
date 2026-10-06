using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Climber : MonoBehaviour
{   

    [SerializeField] public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private int maxLives = 3;
    private int currLives;
    private float invincibilityDur = 1.5f;
    private bool isInvincible = false;
    private Vector3 startPos;
    private SpriteRenderer spriteRender; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   Debug.Log("test");
        rb = GetComponent<Rigidbody2D>();
        currLives = maxLives;
        startPos = transform.position;
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("collision");
        if(isInvincible) return;
        
        GameObject collideWith = coll.gameObject;
        if(collideWith.CompareTag("LargeRock") || 
           collideWith.CompareTag("medRock") ||
           collideWith.CompareTag("smallRock"))
        {
            if (isInvincible) return;
            LoseLife();
            Destroy(collideWith);
        }
    }

    void LoseLife()
    {
        currLives--;
        if(currLives <= 0)
        {
            SceneManager.LoadScene("Scene_0_");
        }
        else
        {
            transform.position = startPos;
            StartCoroutine(BecomeInvincible());
        }
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float currInvincibleDur = 0f;
        while(currInvincibleDur < invincibilityDur)
        {
            spriteRender.enabled = !spriteRender.enabled;
            yield return new WaitForSeconds(0.15f);
            currInvincibleDur += 0.15f;
        }

        spriteRender.enabled = true;
        isInvincible = false;
    }
}

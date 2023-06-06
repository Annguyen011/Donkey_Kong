using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] runSprite;
    [SerializeField] private Sprite climpSprite;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpStrength;
    [SerializeField] private Collider2D[] results;
    [SerializeField] private Collider2D collider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 direction;

    private int spriteIndex;
    private bool grounded;
    private bool climbing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        results = new Collider2D[4];
    }
    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f / 12f, 1f / 12f);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    void Update()
    {
        CheckCollision();
        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        //direction.y = Input.GetAxis("Vertical") * moveSpeed;
        direction.y = Mathf.Max(direction.y, -1f);
        if (direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime);
    }
    public void CheckCollision()
    {
        grounded = false;
        climbing = false;
        Vector2 size = collider.bounds.size;
        size.y += .1f;
        size.x /= 2f;
        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);
        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;
            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                grounded = hit.transform.position.y < (transform.position.y - .5f);
                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }
    public void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climpSprite;
        }
        else if (direction.x != 0f)
        {
            spriteIndex++;
            if (spriteIndex >= runSprite.Length)
            {
                spriteIndex = 0;
            }
            spriteRenderer.sprite = runSprite[spriteIndex];
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collider.CompareTag("Objective"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelComplete();
        }
        else if (collider.CompareTag("Obstacle"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelFail();

        }
    }
}

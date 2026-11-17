using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    
    private float horizontal;
    [Header("Movement Stats")]

    [SerializeField] public float speed = 15f;
    [SerializeField] public float jumpingPower = 16f;
    [SerializeField] private bool isFacingRight = true;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocityY = jumpingPower;
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = horizontal * speed;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position + (Vector3)groundCheckOffset, groundCheckRadius, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            sprite.flipX =! isFacingRight;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere (transform.position + (Vector3)groundCheckOffset, groundCheckRadius);
    }
}

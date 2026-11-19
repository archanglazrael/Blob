using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    
    private float horizontal;
    [Header("Movement Stats")]

    [SerializeField] public float speed = 15f;
    [SerializeField] public float jumpingPower = 16f;
    [SerializeField] private bool isFacingRight = false;

    [Header("Dash Stats")]
    [SerializeField] private float dashPower;    
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    [SerializeField] public int dashCharges = 1;
    private bool canDash = true;
    private bool isDashing = false;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    [Header("Ground Check")]
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;


    [Header("Wall Check1")]
    [SerializeField] private Vector2 wallCheckOffset1;
    [SerializeField] private float wallCheckRadius1;
    
    [Header("Wall Check2")]
    [SerializeField] private Vector2 wallCheckOffset2;
    [SerializeField] private float wallCheckRadius2;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded() && !isDashing)
        {
            rb.linearVelocityY = jumpingPower;
        }
        else if (Input.GetButtonDown("Jump") && IsOnWall() && !isDashing)
        {
            rb.linearVelocityY = jumpingPower;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && (dashCharges > 0) && !isDashing)
        {
            
            StartCoroutine(Dash());
        }
        Flip();
    }

    private float getDirection()
    {
        return sprite.flipX ? 1f : -1f;
    }
    private IEnumerator Dash()
    {
        bool airDash = false;
        if (IsGrounded())
            airDash = false;
        else
            airDash = true;
        canDash = false;
        isDashing = true;
        dashCharges--;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float dashDirection = getDirection();
        rb.linearVelocity = new Vector2(-dashDirection * dashPower, 0f);
        Debug.Log("i dashed");
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        if (!airDash)
        {
            dashCharges++;
        } 
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        rb.linearVelocityX = horizontal * speed;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position + (Vector3)groundCheckOffset, groundCheckRadius, groundLayer);
    }

    

    private bool IsOnWall()
    {
        if (Physics2D.OverlapCircle(transform.position + (Vector3)wallCheckOffset1, wallCheckRadius1, groundLayer) || Physics2D.OverlapCircle(transform.position - (Vector3)wallCheckOffset1, wallCheckRadius1, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
        
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
        Gizmos.DrawWireSphere (transform.position + (Vector3)wallCheckOffset1, wallCheckRadius1);
        Gizmos.DrawWireSphere (transform.position - (Vector3)wallCheckOffset1, wallCheckRadius1);
        Gizmos.DrawLine(transform.position, transform.position * getDirection());

    }
}

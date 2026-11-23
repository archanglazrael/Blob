using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    
    private float horizontal;
    [Header("Movement Stats")]

    [SerializeField] public float topSpeed = 15f;
    [SerializeField] public float groundAcceleration = 29f;
    [SerializeField] public float aerialAcceleration = 0f;
    [SerializeField] public float jumpingPower = 16f;
    [SerializeField] private int jumpCount = 2;
    //[SerializeField] private bool canDoubleJump;
    //[SerializeField] private float jumpDelay = 2;
    private bool isFacingRight = false;
    private int currentJumpCount;

    [Header("Dash Stats")]
    [SerializeField] private float dashPower;    
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    //[SerializeField] public int dashCharges = 1;
    private bool canDash = true;
    private bool isDashing = false;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    [Header("Ground Check")]
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;


    [Header("Wall Check")]
    [SerializeField] private Vector2 wallCheckOffset1;
    [SerializeField] private float wallCheckRadius1;

    private bool CanJump => currentJumpCount > 0;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        currentJumpCount = jumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded() && !isDashing && CanJump)
        {
            rb.linearVelocityY = jumpingPower;
        }
        else if (Input.GetButtonDown("Jump") && IsOnWall() && !isDashing && CanJump)
        {
            rb.linearVelocityY = jumpingPower;

        }
        else if (Input.GetButtonDown("Jump") && (!IsGrounded() && !IsOnWall()) && !isDashing && CanJump)
        {
            currentJumpCount--;
            Debug.Log("-jump");
            rb.linearVelocityY = jumpingPower;
        }

        if (IsGrounded())
        {
                Debug.Log("great oogily boogily");
                currentJumpCount = jumpCount;

        }



        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            
            StartCoroutine(Dash());
        }
        Flip();
    }


    //IEnumerator JumpTime()
    //{
    //    if ((currentJumpCount > 0) && !IsGrounded() && !IsOnWall())
    //    {
    //        yield return new WaitForSeconds(jumpDelay);
    //        if (currentJumpCount <= 0 && jumpDelay > 0)
    //        {
    //            canJump = true;
    //        }
    //        else
    //        {
    //            canJump = false;

    //        }
    //        if (jumpDelay = 0)
    //        {

    //        }

    //    }
            
        
    //}

    private float getDirection()
    {
        return sprite.flipX ? 1f : -1f;
    }
    private IEnumerator Dash()
    {
        bool airDash = !IsGrounded();

        canDash = false;
        isDashing = true;
        //dashCharges--;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float dashDirection = getDirection();
        rb.linearVelocity = new Vector2(-dashDirection * dashPower, 0f);
        Debug.Log("i dashed");
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        while (!canDash)
        {
            if (IsGrounded() || IsOnWall())
                canDash = true;

            yield return null;
        }
        

        //if (!airDash)
        //{
            //dashCharges++;
        //} 
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        rb.AddForceX(horizontal * (IsGrounded() ? groundAcceleration: aerialAcceleration));

        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -topSpeed, topSpeed);
        
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
        //Gizmos.DrawLine(transform.position, transform.position * getDirection());

    }
}

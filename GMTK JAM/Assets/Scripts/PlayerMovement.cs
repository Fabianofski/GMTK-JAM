using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    Rigidbody2D rb2d;
    Animator animator;
    SpriteRenderer spriteRenderer;

    [Header("Movement")]
    Vector2 input;
    [SerializeField] IntConstant speedConstant;
    [SerializeField] BoolVariable canMove;

    [Header("Jump")]
    [SerializeField] IntConstant JumpForceConstant;
    [SerializeField] IntConstant JumpAmountConstant;
    int jumpsLeft;
    [SerializeField] FloatConstant fallMultiplierConstant;
    [SerializeField] FloatConstant lowJumpMultiplierConstant;
    [SerializeField] FloatConstant CoyoteTimeConstant;
    [SerializeField] bool PlayerIsPressingJump;

    [Header("Ground Check")]
    [SerializeField] Transform feet;
    [SerializeField] float radius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask LevelBitLayer;
    bool PlayerIsGrounded;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        jumpsLeft = JumpAmountConstant.Value;
        canMove.Reset();
    }

    private void FixedUpdate()
    {
        if (!canMove.Value) 
        {
            input = Vector2.zero;
            Move();
            return;
        }

        CheckParent();
        Move();
        BetterJump();
        CheckIfPlayerIsGrounded();
        Animation();
    }

    #region Movement
    public void OnMove(InputValue _value)
    {
        input = _value.Get<Vector2>();
        spriteRenderer.flipX = input.x < 0;
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(input.x * speedConstant.Value, rb2d.velocity.y);
    }
    #endregion

    #region Jumping
    public void OnJump(InputValue _value)
    {
        if (!canMove.Value)
            return;

        PlayerIsPressingJump = _value.isPressed;
        TryToPerformJump();
    }

    private void TryToPerformJump()
    {
        if (jumpsLeft > 0 && PlayerIsPressingJump)
        {
            jumpsLeft--;
            GetComponentInChildren<PlayerSounds>().PlayJumpSound();
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(Vector2.up * JumpForceConstant.Value);
        }
    }

    void CheckIfPlayerIsGrounded()
    {
        if (Physics2D.OverlapBox(feet.position, new Vector2(.5f, radius) ,0, groundLayer))
        {
            if (PlayerIsPressingJump) return;
            PlayerIsGrounded = true;
            jumpsLeft = JumpAmountConstant.Value;
        }
        else
            Invoke("DisableJump", CoyoteTimeConstant.Value);
    }

    void DisableJump()
    {
        PlayerIsGrounded = Physics2D.OverlapBox(feet.position, new Vector2(.5f, radius), 0, groundLayer);
    }

    void BetterJump()
    {
        if (rb2d.velocity.y < 0)
            rb2d.velocity += vel(fallMultiplierConstant.Value);
        else if (rb2d.velocity.y > 0 && !PlayerIsPressingJump)
            rb2d.velocity += vel(lowJumpMultiplierConstant.Value);
    }
    Vector2 vel(float x)
    {
        return Vector2.up * Physics2D.gravity.y * (x - 1) * Time.fixedDeltaTime;
    }
    #endregion

    void Animation()
    {
        float _vel = rb2d.velocity.y;
        bool grounded = Physics2D.OverlapBox(feet.position, new Vector2(transform.localScale.x, radius), 0, groundLayer);

        animator.SetBool("PlayerIsFalling", _vel < 0 && !grounded);
        animator.SetBool("PlayerIsJumping", _vel > 0 && !grounded);
        animator.SetBool("PlayerIsWalking", input.x != 0);
    }

    private void CheckParent()
    {
        Collider2D collision = Physics2D.OverlapCircle(transform.position + new Vector3(0, .5f, 0), .5f, LevelBitLayer);

        if (collision)
            transform.parent = collision.gameObject.transform;
        else
            transform.parent = null;
    }

    public void ConstrainPlayer(bool _state)
    {
        rb2d.isKinematic = _state;

        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = _state ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(feet.position, new Vector2(.5f, radius));
    }

}

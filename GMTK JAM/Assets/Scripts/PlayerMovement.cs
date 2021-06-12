using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    Rigidbody2D rb2d;

    [Header("Movement")]
    Vector2 input;
    [SerializeField] IntConstant speedConstant;
    [SerializeField] BoolVariable canMove;

    [Header("Jump")]
    [SerializeField] IntConstant JumpForceConstant;
    [SerializeField] FloatConstant fallMultiplierConstant;
    [SerializeField] FloatConstant lowJumpMultiplierConstant;
    [SerializeField] FloatConstant CoyoteTimeConstant;
    bool PlayerIsPressingJump;

    [Header("Ground Check")]
    [SerializeField] Transform feet;
    [SerializeField] float radius;
    [SerializeField] LayerMask groundLayer;
    bool PlayerIsGrounded;

    [SerializeField] BoolEventReference FireEvent;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void OnFire(InputValue _value)
    {
        FireEvent.Event.Raise(_value.isPressed);
    }

    private void FixedUpdate()
    {
        if (!canMove.Value)
            return;

        Move();
        BetterJump();
        CheckIfPlayerIsGrounded();
    }

    #region Movement
    public void OnMove(InputValue _value)
    {
        input = _value.Get<Vector2>();
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
        if (PlayerIsGrounded && PlayerIsPressingJump)
        {
            Debug.Log("Jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(Vector2.up * JumpForceConstant.Value);
        }
    }

    void CheckIfPlayerIsGrounded()
    {
        if (Physics2D.OverlapBox(feet.position, new Vector2(transform.localScale.x, radius) , groundLayer))
            PlayerIsGrounded = true;
        else
            Invoke("DisableJump", CoyoteTimeConstant.Value);
    }

    void DisableJump()
    {
        PlayerIsGrounded = Physics2D.OverlapCircle(feet.position, radius, groundLayer);
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
}

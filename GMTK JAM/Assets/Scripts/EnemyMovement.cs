using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float movedir = 1;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    [SerializeField] float speed;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        rb2d.velocity = new Vector2(movedir * speed, rb2d.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Turn(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Turn(collision);
    }

    private void Turn(Collision2D collision)
    {
        foreach (ContactPoint2D point in collision.contacts)
        {
            if (Mathf.RoundToInt(point.normal.x) != 0)
            {
                movedir = Mathf.RoundToInt(point.normal.x);
                spriteRenderer.flipX = movedir == 1;
                return;
            }
        }
    }

    public void ConstrainEnemy(bool _state)
    {
        rb2d.isKinematic = _state;

        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = _state ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.FreezeRotation;
    }
}

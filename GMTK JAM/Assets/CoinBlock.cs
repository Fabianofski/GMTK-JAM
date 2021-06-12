using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlock : MonoBehaviour
{
    [SerializeField] LeanTweenType TweenMoveType;
    [SerializeField] float CoinHeight;
    [SerializeField] Sprite usedBlock;
    bool collected;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal != Vector2.up || collected) return;

        collected = true;
        LeanTween.moveY(gameObject, transform.position.y + CoinHeight, .5f).setEase(TweenMoveType);
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = usedBlock;
    }

}

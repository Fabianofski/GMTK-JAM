using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityAtoms.BaseAtoms;

public class Block : MonoBehaviour
{
    [SerializeField] LeanTweenType TweenMoveType;
    [SerializeField] float CoinHeight;
    [SerializeField] Sprite altBlock;
    Sprite ogBlock;
    Vector2 ogpos;
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject HitSound;
    [SerializeField] bool isOneTimeUse = true;
    [SerializeField] BoolUnityEvent SwitchEvent;
    [SerializeField] BoolUnityEvent NotSwitchEvent;
    bool collected;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ogBlock = spriteRenderer.sprite;
        ogpos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal != Vector2.up) return;
        if (collected && isOneTimeUse) return;

        collected = true;

        GameObject _sound = Instantiate(HitSound);
        _sound.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.4f);
        Destroy(_sound, 1f);

        transform.position = ogpos;
        LeanTween.moveY(gameObject, ogpos.y + CoinHeight, .5f).setEase(TweenMoveType);

        if(GetComponent<Animator>())
            GetComponent<Animator>().enabled = false;

        spriteRenderer.sprite = spriteRenderer.sprite != altBlock ? altBlock : ogBlock;
        SwitchEvent.Invoke(spriteRenderer.sprite == altBlock);
        NotSwitchEvent.Invoke(spriteRenderer.sprite != altBlock);
    }

}

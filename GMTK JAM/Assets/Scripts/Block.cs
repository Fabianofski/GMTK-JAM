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
    [SerializeField] BoolVariable LevelIsArranged;
    Sprite ogBlock;
    Vector2 ogpos;
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject HitSound;
    [SerializeField] bool isOneTimeUse = true;
    [SerializeField] GameObject[] ToggleGameObjects;
    bool collected;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ogBlock = spriteRenderer.sprite;
        UpdateOgPos();
    }

    public void UpdateOgPos()
    {
        ogpos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal != Vector2.up) return;
        if ((collected && isOneTimeUse) || LevelIsArranged.Value) return;

        collected = true;

        GameObject _sound = Instantiate(HitSound);
        _sound.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.4f);
        Destroy(_sound, 1f);

        transform.position = new Vector2(transform.position.x ,ogpos.y);
        LeanTween.moveY(gameObject, ogpos.y + CoinHeight, .5f).setEase(TweenMoveType);

        if(GetComponent<Animator>())
            GetComponent<Animator>().enabled = false;

        spriteRenderer.sprite = spriteRenderer.sprite != altBlock ? altBlock : ogBlock;
        foreach (GameObject _gameObject in ToggleGameObjects)
            _gameObject.SendMessage("Toggle");
    }

}

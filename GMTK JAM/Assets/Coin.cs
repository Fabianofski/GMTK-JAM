using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class Coin : MonoBehaviour
{
    [SerializeField] IntEventReference CollectedCoinEvent;
    [SerializeField] int value;
    [SerializeField] LeanTweenType TweenFadeType;
    [SerializeField] LeanTweenType TweenMoveType;
    [SerializeField] float CoinHeight;
    [SerializeField] GameObject CoinSound;
    bool collected;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected) return;

        CollectedCoinEvent.Event.Raise(value);
        collected = true;

        GameObject _sound = Instantiate(CoinSound);
        _sound.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.4f);
        Destroy(_sound, 1f);

        LeanTween.moveY(gameObject, transform.position.y + CoinHeight, .5f).setEase(TweenMoveType);
        LeanTween.alpha(gameObject, 0f, .5f).setEase(TweenFadeType).setOnComplete(() => { Destroy(gameObject); });
    }
}

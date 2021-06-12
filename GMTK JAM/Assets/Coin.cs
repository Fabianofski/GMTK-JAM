using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class Coin : MonoBehaviour
{
    [SerializeField] IntEventReference CollectedCoinEvent;
    [SerializeField] int value;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectedCoinEvent.Event.Raise(value);
    }
}

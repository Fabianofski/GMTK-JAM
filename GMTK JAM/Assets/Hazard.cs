using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class Hazard : MonoBehaviour
{
    [SerializeField] VoidEvent PlayerDieEvent;
    [SerializeField] BoolVariable LevelIsArranged;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LevelIsArranged.Value) return;

        if (collision.collider.CompareTag("Player"))
        {
            PlayerDieEvent.Raise();
        }
    }
}

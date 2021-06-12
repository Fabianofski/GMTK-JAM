using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityAtoms.BaseAtoms;

public class Goal : MonoBehaviour
{

    [SerializeField] int TotalCoins;
    [SerializeField] int CoinsCollected = 0;
    [SerializeField] VoidEvent ReachedGoalEvent;
    [SerializeField] BoolVariable GoalIsUnlocked;

    private void Awake()
    {
        TotalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    public void CollectCoin(int _amount)
    {
        CoinsCollected += _amount;

        if (TotalCoins >= CoinsCollected)
            GoalIsUnlocked.Value = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GoalIsUnlocked.Value)
            ReachedGoalEvent.Raise();
    }

}

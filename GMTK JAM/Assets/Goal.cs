using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityAtoms.BaseAtoms;

public class Goal : MonoBehaviour
{

    [SerializeField] int TotalCoins;
    [SerializeField] int CoinsCollected = 0;
    [SerializeField] BoolEvent ReachedGoalEvent;
    [SerializeField] BoolVariable GoalIsUnlocked;
    [SerializeField] BoolEvent GoalIsUnlockedEvent;

    private void Awake()
    {
        TotalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        GoalIsUnlocked.Reset();
    }

    public void CollectCoin(int _amount)
    {
        CoinsCollected += _amount;

        if (CoinsCollected >= TotalCoins)
        {
            GoalIsUnlocked.Value = true;
            GoalIsUnlockedEvent.Raise(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GoalIsUnlocked.Value)
        {
            Debug.Log("Reached Goal");
            ReachedGoalEvent.Raise(true);
        }
    }

}

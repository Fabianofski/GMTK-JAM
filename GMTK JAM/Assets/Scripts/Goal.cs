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
    [SerializeField] BoolVariable GameEnded;
    [SerializeField] BoolVariable LevelIsArranged;
    [SerializeField] BoolEvent GoalIsUnlockedEvent;

    private void Awake()
    {
        TotalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        GoalIsUnlocked.Reset();
        GameEnded.Reset();

        CheckUnlockCondition();
    }

    public void CollectCoin(int _amount)
    {
        CoinsCollected += _amount;
        CheckUnlockCondition();
    }

    private void CheckUnlockCondition()
    {
        if (CoinsCollected >= TotalCoins)
        {
            GoalIsUnlocked.Value = true;
            GoalIsUnlockedEvent.Raise(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GoalIsUnlocked.Value && !LevelIsArranged.Value)
        {
            collision.transform.position = transform.position;
            Debug.Log("Reached Goal");
            ReachedGoalEvent.Raise(true);
            GameEnded.Value = true;
        }
    }

}

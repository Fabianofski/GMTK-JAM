using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSounds : MonoBehaviour
{

    [SerializeField] GameObject[] FootStepSounds;
    [SerializeField] GameObject JumpSound;
    [SerializeField] Transform startPos;
    [SerializeField] UnityEvent SpawnPlayer;

    private void Awake()
    {
        
    }

    public void PlayFootStepSound()
    {
        GameObject _sound = Instantiate(FootStepSounds[Random.Range(0, FootStepSounds.Length)]);
        _sound.GetComponent<AudioSource>().pitch = Random.Range(1.4f, 2f);
        Destroy(_sound, 1f);
    }

    public void PlayJumpSound()
    {
        GameObject _sound = Instantiate(JumpSound);
        _sound.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.4f);
        Destroy(_sound, 1f);
    }


    public void Die()
    {
        transform.parent.position = startPos.position;
        transform.parent.parent = startPos.parent;

        SpawnPlayer.Invoke();
    }
}

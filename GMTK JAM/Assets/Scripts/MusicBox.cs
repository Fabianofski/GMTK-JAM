using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicBox : MonoBehaviour
{
    [SerializeField] AudioClip GrassSong;
    [SerializeField] AudioClip DesertSong;
    [SerializeField] AudioClip TownSong;
    [SerializeField] AudioClip CaveSong;
    AudioSource audioSource;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("MusicBox").Length > 1)
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        PlayTrack();
    }

    private void OnLevelWasLoaded(int level)
    {
        audioSource = GetComponent<AudioSource>();
        PlayTrack();
    }

    void PlayTrack()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (scene.Contains("Grass"))
        {
            if (audioSource.clip != GrassSong)
                SwitchTrack(GrassSong);
        }
        else if (scene.Contains("Desert"))
        {
            if (audioSource.clip != DesertSong)
                SwitchTrack(DesertSong);
        }
        else if (scene.Contains("Town"))
        {
            if (audioSource.clip != TownSong)
                SwitchTrack(TownSong);
        }
        else if (scene.Contains("Cave"))
        {
            if (audioSource.clip != CaveSong)
                SwitchTrack(CaveSong);
        }
    }

    void SwitchTrack(AudioClip _clip)
    {
        float _vol = audioSource.volume;

        LeanTween.value(_vol, 0f, 1f).setOnUpdate((float value) =>
        {
            audioSource.volume = value;
        }).setOnComplete(() => 
        {
            audioSource.clip = _clip;
            LeanTween.value(0f, _vol, 1f).setOnUpdate((float value) =>
            {
                audioSource.volume = value;
                audioSource.Play();
            });
        });
    }
}

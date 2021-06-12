using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;

public class UI_Events:MonoBehaviour
{
    public AudioMixer mixer { private get; set; }
    [SerializeField] VoidEvent UpdateUIEvent;

    private void Awake()
    {
        UpdateUIEvent.Raise();
    }

    public void ToggleUIElement(GameObject _UIElement)
    {
        _UIElement.SetActive(!_UIElement.activeSelf);
    }
    
    public void ToggleUIElements(GameObject[] _UIElements)
    {
        foreach(GameObject _UIElement in _UIElements)
            _UIElement.SetActive(!_UIElement.activeSelf);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleFullscreen(bool _toggle)
    {
        Screen.fullScreen = _toggle;
    }

    public void ChangeVolume(float _volume)
    {
        PlayerPrefs.SetFloat(mixer.name, _volume);
        PlayerPrefs.Save();

        _volume = Mathf.Log(_volume) * 20;
        mixer.SetFloat("volume", _volume);
    }

    public void UpdateSlider(Slider _slider)
    {
        _slider.value = PlayerPrefs.GetFloat(mixer.name, 1);
    }

    public void PlaySound(GameObject _sound)
    {
        _sound = Instantiate(_sound);
        _sound.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.4f);
        Destroy(_sound, 1f);
    }

}

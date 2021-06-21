using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UI_Events:MonoBehaviour
{
    public AudioMixer mixer { private get; set; }
    [SerializeField] VoidEvent UpdateUIEvent;
    [SerializeField] GameObject PopSound;

    [Header("CheckMouseInput")]
    Vector2 oldMousePos;
    bool mouseIsMoving;
    GameObject selectedElement;
    [SerializeField] float threshold;
    [SerializeField] bool checkMouseInput = false;
    bool cooldown;
    EventSystem eventSystem; 

    private void Start()
    {
        if (UpdateUIEvent)
            UpdateUIEvent.Raise();
        eventSystem = EventSystem.current;
        selectedElement = eventSystem.currentSelectedGameObject;
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

        PlaySound(PopSound);
        _volume = Mathf.Log(_volume) * 20;
        mixer.SetFloat("volume", _volume);
    }

    public void UpdateSlider(Slider _slider)
    {
        float _volume = PlayerPrefs.GetFloat(mixer.name, 1);
        _slider.value = _volume;

        _volume = Mathf.Log(_volume) * 20;
        mixer.SetFloat("volume", _volume);
    }

    public void PlaySound(GameObject _sound)
    {
        _sound = Instantiate(_sound);
        _sound.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.4f);
        Destroy(_sound, 1f);
    }

    private void Update()
    {
        if(checkMouseInput)
            OnMouseMove();
    }

    public void OnMouseMove()
    {
        float _MouseMag = (Mouse.current.position.ReadValue() - oldMousePos).magnitude;
        mouseIsMoving = Mathf.Abs(_MouseMag) > threshold;
        GameObject _currentGameObject = eventSystem.currentSelectedGameObject;

        if(mouseIsMoving && _currentGameObject != null)
        {
            selectedElement = _currentGameObject;
            eventSystem.SetSelectedGameObject(null);
            if(!cooldown)
                Cooldown();
        }
        else if (_currentGameObject != null)
            selectedElement = _currentGameObject;
        else if (!mouseIsMoving && !cooldown)
        {
            eventSystem.SetSelectedGameObject(selectedElement);
            Cooldown();
        }

        oldMousePos = Mouse.current.position.ReadValue();
    }

    void Cooldown()
    {
        cooldown = true;
        Invoke("ResetCooldown", .5f);
    }

    void ResetCooldown() => cooldown = false;

}

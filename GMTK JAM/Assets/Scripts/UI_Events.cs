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
    [SerializeField] GameObject selectedElement;
    [SerializeField] float threshold;
    [SerializeField] bool checkMouseInput = false;
    EventSystem eventSystem;
    [SerializeField] InputActionAsset inputAsset;
    [SerializeField] BoolVariable PlayerCanMove;
    [SerializeField] BoolVariable LevelIsArranged;
    [SerializeField] LeanTweenType TransitionInType;
    [SerializeField] LeanTweenType TransitionOutType;

    private void Start()
    {
        if (UpdateUIEvent)
            UpdateUIEvent.Raise();

        Cursor.visible = SceneManager.GetActiveScene().buildIndex == 0;

        if (!checkMouseInput) return;
        eventSystem = EventSystem.current;
        selectedElement = eventSystem.currentSelectedGameObject;
        inputAsset.Enable();
        inputAsset.FindActionMap("UI").FindAction("Navigate").performed += _ => OnKeyboardMove();
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

    public void UpdateFullscreenToggle(Toggle _toggle)
    {
        _toggle.isOn = Screen.fullScreen;
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
        GameObject _selectedElement = eventSystem.currentSelectedGameObject;

        if(mouseIsMoving && _selectedElement)
        {
            selectedElement = _selectedElement;
            eventSystem.SetSelectedGameObject(null);
        }

        oldMousePos = Mouse.current.position.ReadValue();
    }

    private void OnKeyboardMove()
    {
        GameObject _selectedElement = eventSystem.currentSelectedGameObject;
        if(!_selectedElement)
            eventSystem.SetSelectedGameObject(selectedElement);
    }

    public void PauseGame(bool _isPaused)
    {
        PlayerCanMove.Value = !_isPaused && !LevelIsArranged.Value;
        Cursor.visible = _isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
    }

    public void TransitionUIElement(GameObject _UIElement)
    {
        bool _popIn = !_UIElement.activeSelf;
        _UIElement.SetActive(true);

        LeanTween.scale(_UIElement, _popIn ? Vector2.one : Vector2.zero, .3f)
            .setEase(_popIn ? TransitionInType : TransitionOutType)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => 
            { 
                if (!_popIn) 
                    _UIElement.SetActive(false); 
            });

    }
}

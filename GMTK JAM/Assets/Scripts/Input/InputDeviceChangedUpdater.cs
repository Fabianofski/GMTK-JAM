using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputDeviceChangedUpdater : MonoBehaviour
{
    
    [System.Serializable]
    public class InputImage
    {
        public Image image;
        public Sprite KeyboardSprite;
        public Sprite GamepadSprite;
    }
    
    [System.Serializable]
    public class InputSpriteRenderer
    {
        public SpriteRenderer spriteRenderer;
        public Sprite KeyboardSprite;
        public Sprite GamepadSprite;
    }
    
    [System.Serializable]
    public class InputButton
    {
        public Button button;
        public Sprite KeyboardSprite;
        public Sprite KeyboardSpritePressed;
        public Sprite GamepadSprite;
        public Sprite GamepadSpritePressed;
    }

    [SerializeField] InputImage[] inputImages;
    [SerializeField] InputSpriteRenderer[] inputSpriteRenderers;
    [SerializeField] InputButton[] inputButtons;

    public void OnInputDeviceChanged(bool _isGamepad)
    {
        UpdateImages(_isGamepad);
        UpdateSpriteRenderers(_isGamepad);
        UpdateButtons(_isGamepad);
    }

    private void UpdateImages(bool _isGamepad)
    {
        foreach (InputImage inputImage in inputImages)
        {
            inputImage.image.sprite = _isGamepad ? inputImage.GamepadSprite : inputImage.KeyboardSprite;
        }
    }

    private void UpdateSpriteRenderers(bool _isGamepad)
    {
        foreach (InputSpriteRenderer inputSpriteRenderer in inputSpriteRenderers)
        {
            inputSpriteRenderer.spriteRenderer.sprite = _isGamepad ? inputSpriteRenderer.GamepadSprite : inputSpriteRenderer.KeyboardSprite;
        }
    }
    
    private void UpdateButtons(bool _isGamepad)
    {
        foreach (InputButton inputButton in inputButtons)
        {
            Sprite _sprite = _isGamepad ? inputButton.GamepadSprite : inputButton.KeyboardSprite;
            Sprite _spritePressed = _isGamepad ? inputButton.GamepadSpritePressed : inputButton.KeyboardSpritePressed;

            inputButton.button.GetComponent<Image>().sprite = _sprite;

            SpriteState _spriteState = new SpriteState();
            _spriteState.highlightedSprite = _spriteState.pressedSprite = _spriteState.selectedSprite = _spritePressed;
            inputButton.button.spriteState = _spriteState;
        }
    }
}

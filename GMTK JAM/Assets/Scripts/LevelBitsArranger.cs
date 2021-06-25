using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelBitsArranger : MonoBehaviour
{

    [Header("")]
    [SerializeField] BoolVariable LevelIsArranged;
    [SerializeField] LeanTweenType TweenScaleType;
    [SerializeField] BoolVariable PlayerCanMove;
    [SerializeField] BoolVariable GameEnded;

    [Header("Sounds")]
    [SerializeField] GameObject ClickSound;
    [SerializeField] GameObject ReleaseSound;
    [SerializeField] GameObject SplitApartSound;
    [SerializeField] GameObject JoinTogetherSound;

    [Header("Level Bit Properties")]
    [SerializeField] LayerMask levelBitLayer;
    GameObject levelBit;
    Vector2 offset;
    Vector2 originalPos;

    #region SplitMode
    public void SetUpLevelBits(bool _defaultPos)
    {
        if (GameEnded.Value) return;

        PlayerCanMove.Value = !_defaultPos;

        GameObject _sound = Instantiate(_defaultPos ? SplitApartSound : JoinTogetherSound);
        Destroy(_sound, 1f);

        foreach (Transform _child in transform)
        {
            FadeAlpha(_defaultPos, _child);

            if (_child.name != "BG")
            {
                if (_defaultPos)
                    ScaleDown(_child);
                else
                {
                    LevelIsArranged.Value = false;
                    if (levelBit)
                        ReleaseLevelBit();
                    ScaleUp(_child);
                }
            }
        }
    }

    private void FadeAlpha(bool _defaultPos, Transform _child)
    {
        SpriteRenderer _spriteRenderer = _child.GetComponent<SpriteRenderer>();
        LeanTween.value(0f, 1f, .5f).setEase(TweenScaleType).setOnUpdate((float value) =>
        {
            float _a = _defaultPos ? value : 1 - value;
            Color _color = _spriteRenderer.color;
            _color.a = _a;
            _spriteRenderer.color = _color;
        });
    }

    private void ScaleDown(Transform _child)
    {
        LeanTween.value(1f, 0.8f, .5f).setEase(TweenScaleType).setOnUpdate((float value) =>
        { _child.localScale = new Vector2(value, value); }).setOnComplete(() =>
        {
            LevelIsArranged.Value = true;
        });
    }

    private void ScaleUp(Transform _child)
    {
        LeanTween.value(0.8f, 1f, .5f).setEase(TweenScaleType).setOnUpdate((float value) =>
        { _child.localScale = new Vector2(value, value); });
    }
    #endregion

    #region MouseControls

    public void Click()
    {
        if (!LevelIsArranged.Value) return;

        if (!levelBit)
            ClickLevelBit();
        else if(levelBit)
            ReleaseLevelBit();
    }

    private void ClickLevelBit()
    {
        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        levelBit = GetLevelBit(_mousePos);
        if (levelBit)
        {
            GameObject _sound = Instantiate(ClickSound);
            _sound.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.4f);
            Destroy(_sound, 1f);

            levelBit.GetComponent<SortingGroup>().sortingOrder += 1;
            levelBit.GetComponent<LevelBitPreview>().isDragged = true;
            originalPos = levelBit.transform.position;
            offset = _mousePos - originalPos;
        }
    }
    #endregion
    private void ReleaseLevelBit()
    {
        GameObject _sound = Instantiate(ReleaseSound);
        _sound.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.4f);
        Destroy(_sound, 1f);

        levelBit.GetComponent<SortingGroup>().sortingOrder -= 1;
        levelBit.GetComponent<LevelBitPreview>().isDragged = false;

        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        GameObject _levelBit = GetLevelBit(_mousePos);

        if (_levelBit)
        {
            levelBit.transform.position = _levelBit.transform.position;
            _levelBit.transform.position = originalPos;
        }
        else
            levelBit.transform.position = originalPos;

        levelBit = null;
    }

    GameObject GetLevelBit(Vector2 _mousePos)
    {
        RaycastHit2D _hit2d = Physics2D.Raycast(_mousePos, Vector2.zero, 0f, levelBitLayer);

        if (_hit2d)
            return _hit2d.collider.gameObject;
        else
            return null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;

public class LevelBitsArranger : MonoBehaviour
{

    [Header("")]
    [SerializeField] BoolVariable LevelIsArranged;
    [SerializeField] LeanTweenType TweenScaleType;

    [Header("Level Bit Properties")]
    [SerializeField] LayerMask levelBitLayer;
    GameObject levelBit;
    Vector2 offset;
    Vector2 originalPos;

    private void Awake()
    {
        Invoke("Temp", 2f);
    }

    void Temp()
    {
        SetUpLevelBits(true);
    }

    public void SetUpLevelBits(bool _defaultPos)
    {
        foreach (Transform _child in transform)
        {
            SpriteRenderer _spriteRenderer = _child.GetComponent<SpriteRenderer>();
            LeanTween.value(0f, 1f, .5f).setEase(TweenScaleType).setOnUpdate((float value) =>
            {
                float _a = _defaultPos ? value : 1 - value;
                Color _color = _spriteRenderer.color;
                _color.a = _a;
                _spriteRenderer.color = _color;
            });

            if (_defaultPos)
                LeanTween.value(1f, 0.8f, .5f).setEase(TweenScaleType).setOnUpdate((float value) =>
                { _child.localScale = new Vector2(value, value); });
            else
                LeanTween.value(0.8f, 1f, .5f).setEase(TweenScaleType).setOnUpdate((float value) =>
                { _child.localScale = new Vector2(value, value); }).setOnComplete(() => { LevelIsArranged.Value = false; }) ;
        }

    }

    private void Update()
    {
        if(levelBit)
            MoveLevelBit();
    }

    void MoveLevelBit()
    {
        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _mousePos -= offset;
        levelBit.transform.position = _mousePos;
    }

    public void Click(bool _isPressed)
    {
        if (!LevelIsArranged.Value) return;

        if (_isPressed)
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
            originalPos = levelBit.transform.position;
            offset = _mousePos - originalPos;
        }
    }

    private void ReleaseLevelBit()
    {
        levelBit.layer = 0;
        Collider2D _col = Physics2D.OverlapBox(levelBit.transform.position, levelBit.transform.localScale, 0f, levelBitLayer);
        levelBit.layer = 6;

        if (_col)
        {
            levelBit.transform.position = _col.transform.position;
            _col.transform.position = originalPos;
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

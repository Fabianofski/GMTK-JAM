using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelBitsArranger : MonoBehaviour
{

    [Header("Level Bit Properties")]
    [SerializeField] LayerMask levelBitLayer;
    GameObject levelBit;
    Vector2 offset;
    Vector2 originalPos;

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

    public void Click(InputAction.CallbackContext _context)
    {
        if (_context.performed)
            ClickLevelBit();
        else if (_context.canceled && levelBit)
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

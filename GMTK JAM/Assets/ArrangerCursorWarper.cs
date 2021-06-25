using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem.LowLevel;

public class ArrangerCursorWarper : MonoBehaviour
{
    [SerializeField] InputActionAsset inputAsset;
    [SerializeField] List<Vector2> levelBits;
    int index;

    [SerializeField] BoolReference LevelIsArranged;

    private void Awake()
    {
        inputAsset.Enable();
        inputAsset.FindActionMap("UI").FindAction("Navigate").performed += _context => Navigate(_context);

        levelBits = new List<Vector2>();
        foreach (Transform child in transform)
            if (child.name != "BG")
            {
                Vector2 _childPos = child.position;
                levelBits.Add(_childPos);
            }
    }

    void Navigate(InputAction.CallbackContext _context)
    {
        if (!LevelIsArranged.Value) return;

        Vector2 _input = _context.ReadValue<Vector2>();
        UpdateIndex(_input);

        Vector2 _screenPos = Camera.main.WorldToScreenPoint(levelBits[index]);
        Mouse.current.WarpCursorPosition(_screenPos);

        InputState.Change(Mouse.current.position, _screenPos); // Update MousePos in Input System
    }

    #region IndexUpdater
    void UpdateIndex(Vector2 _input)
    {
        if (_input.x == -1)
            NextLevelBit();
        else if (_input.x == 1)
            PreviousLevelBit();

        if (_input.y == 1)
            UpperLevelBit();
        else if (_input.y == -1)
            LowerLevelBit();
    }

    void NextLevelBit()
    {
        Vector2 _oldPos = levelBits[index];
        if (index > 0)
            if (_oldPos.y == levelBits[index - 1].y)
                index--;
    }

    void PreviousLevelBit()
    {
        Vector2 _oldPos = levelBits[index];
        if (index < levelBits.Count - 1)
            if (_oldPos.y == levelBits[index + 1].y)
                index++;
    }

    void UpperLevelBit()
    {
        Vector2 _oldPos = levelBits[index];
        List<Vector2> _upperBits = new List<Vector2>();

        foreach (Vector2 _pos in levelBits)
            if (_pos.x == _oldPos.x && _oldPos.y < _pos.y)
                _upperBits.Add(_pos);

        Vector2 _closestPos = ClosestPos(_oldPos, _upperBits);

        index = levelBits.IndexOf(_closestPos);
    }
    
    void LowerLevelBit()
    {
        Vector2 _oldPos = levelBits[index];
        List<Vector2> _upperBits = new List<Vector2>();

        foreach (Vector2 _pos in levelBits)
            if (_pos.x == _oldPos.x && _oldPos.y > _pos.y)
                _upperBits.Add(_pos);

        Vector2 _closestPos = ClosestPos(_oldPos, _upperBits);

        index = levelBits.IndexOf(_closestPos);
    }

    Vector2 ClosestPos(Vector2 _oldPos, List<Vector2> _posList)
    {
        Vector2 _closest = _oldPos;
        float _distance = Mathf.Infinity;

        foreach (Vector2 _pos in _posList)
        {
            float _dist = Vector2.Distance(_oldPos, _pos);
            if (Vector2.Distance(_oldPos, _pos) < _distance)
            {
                _distance = _dist;
                _closest = _pos;
            }
        }

        return _closest;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;

public class LevelBitPreview : MonoBehaviour
{

    [Header("Light Preview")]
    [SerializeField] GameObject preview;
    [SerializeField] GameObject selectPreview;

    [Header("Raycast")]
    [SerializeField] BoolVariable LevelIsArranged;
    [SerializeField] LayerMask layer;
    public bool isDragged;

    private void Update()
    {
        if (!LevelIsArranged.Value) 
        {
            preview.SetActive(false);
            selectPreview.SetActive(false);
            return; 
        }

        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        GameObject _levelBit = GetLevelBit(_mousePos);

        preview.SetActive(_levelBit == gameObject && !isDragged);
        selectPreview.SetActive(isDragged);
    }
    GameObject GetLevelBit(Vector2 _mousePos)
    {
        RaycastHit2D _hit2d = Physics2D.Raycast(_mousePos, Vector2.zero, 0f, layer);

        if (_hit2d)
            return _hit2d.collider.gameObject;
        else
            return null;
    }
}

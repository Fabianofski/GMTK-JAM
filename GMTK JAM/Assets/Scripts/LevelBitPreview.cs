using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityAtoms.BaseAtoms;

public class LevelBitPreview : MonoBehaviour
{

    GameObject preview;
    [SerializeField] BoolVariable LevelIsArranged;
    [SerializeField] LayerMask layer;
    public bool isDragged;

    private void Awake()
    {
        preview = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (!LevelIsArranged.Value) return;

        Collider2D[] _col = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f, layer);

        preview.SetActive(_col.Length > 1 || isDragged);
    }

}

using System;
using Creatures;
using DefaultNamespace;
using UnityEngine;


public class Draggable : MonoBehaviour
{
    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    private static readonly int _outlineThickness = Shader.PropertyToID("_OutlineThickness");
    private Mass _mass;
    private Earth _earth;
    public bool Grabbed = false;

    private void Start()
    {
        _camera = Camera.main;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _mass = GetComponent<Mass>();
        _earth = GetComponentInParent<Earth>();
    }

    private void OnMouseEnter()
    {
        _spriteRenderer.material = PrefabService.Instance.OutlineMaterial;
    }

    private void OnMouseExit()
    {
        _spriteRenderer.material = PrefabService.Instance.SimpleMaterial;
    }

    private void OnMouseDown()
    {
        Grabbed = true;
        if (GetComponent<Human>() != null)
        {
            _earth.HumanDown.Play();
        }
        else if (GetComponent<Building>() != null)
        {
            _earth.BuildingDown.Play();
        }
    }

    private void OnMouseDrag()
    {
        Vector3 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        transform.position = position;
        if (Mathf.Abs(transform.localPosition.x) < _earth.Radius)
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Clamp(transform.localPosition.y, 0, Single.PositiveInfinity), transform.localPosition.z);
    }

    private void OnMouseUp()
    {
        _mass.WannaBonk = true;
        Grabbed = false;
    }
}

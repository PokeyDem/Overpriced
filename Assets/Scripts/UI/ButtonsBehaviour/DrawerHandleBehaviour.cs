using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class DrawerHandleBehaviour : MonoBehaviour, IHoverableObject, IInteractableMenuButton
{

    [SerializeField] private Material _hoverMaterial;
    private Material _baseMaterial;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer  = GetComponent<MeshRenderer>();
        _baseMaterial = _meshRenderer.material;
    }

    public void OnHoverEnter()
    {
        _meshRenderer.material = _hoverMaterial;
        Debug.Log("OnHoverEnter");
    }

    public void OnHoverExit()
    {
        _meshRenderer.material = _baseMaterial;
        Debug.Log("OnHoverExit");
    }

    public void Interact()
    {
        MainMenuManager.Instance.OnDrawerHandleClick();
    }
}

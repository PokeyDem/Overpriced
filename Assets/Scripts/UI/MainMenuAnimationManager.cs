using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenuAnimationManager : SingletonDontDestroyOnLoad<MainMenuAnimationManager>
{

    [SerializeField] private Transform drawer;
    [SerializeField] private Transform handle;
    [SerializeField] private float drawerOpenOffsetY;
    [SerializeField] private float drawerOpeningDuration;
    [SerializeField] private float buttonPressingDuration;
    [SerializeField] private float handleRotationDegree;
    private Vector3 _drawerClosedPos;
    private Vector3 _drawerOpenedPos;
    private bool _isAnimatingButton;
    private new void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _drawerClosedPos = drawer.position;
        _drawerOpenedPos = new Vector3(_drawerClosedPos.x, _drawerClosedPos.y, _drawerClosedPos.z + drawerOpenOffsetY);
    }

    public void PressButton(Transform button)
    {
        if (_isAnimatingButton) return;
        
        _isAnimatingButton = true;
        
        button.DOPunchPosition(new Vector3(0, 0.00018f, -0.000212f), buttonPressingDuration).OnComplete(() =>
        {
            _isAnimatingButton = false;
        });
    }

    public void OpenDrawer()
    {
        drawer.DOMove(_drawerOpenedPos, drawerOpeningDuration);
        RotateHandle(handleRotationDegree);
    }

    public void CloseDrawer()
    {
        drawer.DOMove(_drawerClosedPos, drawerOpeningDuration);
        RotateHandle(-handleRotationDegree);
    }

    private void RotateHandle(float rotationDegree)
    {
        // handle.DORotate(new Vector3(rotationDegree, handle.eulerAngles.y, handle.eulerAngles.z), drawerOpeningDuration, RotateMode.LocalAxisAdd);
        handle.DOLocalRotate(new Vector3(rotationDegree, 0, 0), drawerOpeningDuration, RotateMode.LocalAxisAdd);
    }

    public void RotateHandleWithoutRelatedAction(bool clockwise)
    {
        RotateHandle(clockwise ? -handleRotationDegree : handleRotationDegree);
    }
}

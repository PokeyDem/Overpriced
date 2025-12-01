using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonWithDestroy<CameraController> {
    [SerializeField] private Vector3 _gridPos;
    [SerializeField] private Vector3 _counterPos;
    [SerializeField] private Vector3 _cashRegisterPos;
    [SerializeField] private Quaternion _cashRegisterRot;
    [SerializeField] private Quaternion _gameplayRot;
    [SerializeField] private Quaternion _drawerRot;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _transitionSpeed;
    private Vector3 _targetPos;
    private Quaternion _targetRotation;
    private bool _isInAction;

    private new void Awake(){
        base.Awake();
        _targetPos = _gridPos;
    }

    private void Update(){
        if (_isInAction)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _targetPos, Time.deltaTime * _transitionSpeed);
            _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation, _targetRotation, Time.deltaTime * _transitionSpeed);
            
            float distPos = Vector3.Distance(_camera.transform.position, _targetPos);
            float angleRot = Quaternion.Angle(_camera.transform.rotation, _targetRotation);
            
            if (distPos < 0.001f && angleRot < 0.01f)
            {
                _camera.transform.position = _targetPos;
                _camera.transform.rotation = _targetRotation;
                
                _isInAction = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            SwitchToShop();
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            _targetPos = _gridPos;
            _isInAction = true;
        }
    }

    public void SwitchToCashRegister()
    {
        _targetPos = _cashRegisterPos;
        _targetRotation = _cashRegisterRot;
        _isInAction = true;
    }

    public void SwitchToShop()
    {
        _targetPos = _counterPos;
        _targetRotation = _gameplayRot;
        _isInAction = true;
    }

    public void SwitchToCashRegisterDrawer()
    {
        _targetRotation = _drawerRot;
        _isInAction = true;
    }
}

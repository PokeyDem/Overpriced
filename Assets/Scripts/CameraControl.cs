using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour{
    [SerializeField] private Vector3 _gridPos;
    [SerializeField] private Vector3 _counterPos;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _transitionSpeed;
    private Vector3 _targetPos;

    private void Awake(){
        _targetPos = _gridPos;
    }

    private void Update(){
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _targetPos, Time.deltaTime * _transitionSpeed);
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            _targetPos = _counterPos;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            _targetPos = _gridPos;
        }
    }
}

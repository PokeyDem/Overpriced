using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour{

    [SerializeField] private float _moveSpeed = 5f; 
    [SerializeField] private float _inertiaDecay = 2f;
    [SerializeField] private float _rotationSpeed = 10f;
    private Rigidbody _rb;
    private Vector3 _movementVector;
    private Vector3 _currentVelocity;
    private float _yPos;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _yPos = transform.position.y;
    }

    void Update()
    {
       
        float horizontalInput = Input.GetAxisRaw("Horizontal"); 
        float verticalInput = Input.GetAxisRaw("Vertical");    
        
        _movementVector = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        
        if (_movementVector != Vector3.zero)
        {
            _currentVelocity = _movementVector * _moveSpeed;
        }
        else
        {
            _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, Time.deltaTime * _inertiaDecay);
        }
        
        if (_currentVelocity != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(_currentVelocity, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * _rotationSpeed);
        }
        

    }

    void FixedUpdate(){
        Vector3 newPos = _rb.position + _currentVelocity * Time.fixedDeltaTime;
        newPos.y = _yPos;
        _rb.MovePosition(newPos);
    }
    
}

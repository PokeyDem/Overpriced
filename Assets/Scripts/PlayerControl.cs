using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour{

    [SerializeField] private float _moveSpeed = 2.0f;
    [SerializeField] private float _rotationSpeed = 10.0f;

    private void Update(){
        
        Vector2 input = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W)){
            input.y += 1;
        }

        if (Input.GetKey(KeyCode.S)){
            input.y -= 1;
        }

        if (Input.GetKey(KeyCode.A)){
            input.x -= 1;
        }

        if (Input.GetKey(KeyCode.D)){
            input.x += 1;
        }

        input = input.normalized;

        Vector3 moveDir = new Vector3(input.x, 0f, input.y);
        
        // Debug.Log(_playerDirection);
        
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _rotationSpeed);

        float playerSize = 0.5f;
        float offset = 0.5f;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - offset, transform.position.z);
        Debug.DrawRay(pos, moveDir * (playerSize * 2f), Color.red);
        RaycastHit hit;
        Ray ray = new Ray(pos, moveDir);
        // bool canMove =! Physics.Raycast(pos, moveDir, playerSize * 0.8f);
        bool canMove = !Physics.Raycast(ray, out hit, playerSize * 0.8f) || hit.collider.isTrigger;
        if (canMove) 
            transform.position += moveDir * (_moveSpeed * Time.deltaTime);
    }
}

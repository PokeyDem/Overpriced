using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAudioPlayer : MonoBehaviour{
    private PlayerControl _playerControl;

    [SerializeField] private float _stepInterval;
    [SerializeField] private float _cooldownTime;
    
    private float _stepTimer;
    private float _cooldownTimer;
    private void Awake(){
        _playerControl = GetComponent<PlayerControl>();
    }

    private void Update(){
        _cooldownTimer -= Time.deltaTime;
        if (_playerControl.IsMoving()){
            
            _stepTimer -= Time.deltaTime;
            
            if (_stepTimer <= 0 && _cooldownTimer <= 0){
                
                AudioManager.PlaySound(SoundType.WALK);
                _stepTimer = _stepInterval;
                _cooldownTimer = _cooldownTime;
            }
        }
        else{
            _stepTimer = 0;
        }
    }
}

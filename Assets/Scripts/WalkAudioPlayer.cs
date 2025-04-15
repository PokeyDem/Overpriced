using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAudioPlayer : MonoBehaviour{
    private PlayerControl _playerControl;

    [SerializeField] private float _stepInterval;
    private float _stepTimer;

    private void Awake(){
        _playerControl = GetComponent<PlayerControl>();
    }

    private void Update(){
        if (_playerControl.IsMoving()){
            _stepTimer -= Time.deltaTime;
            if (_stepTimer <= 0){
                AudioManager.PlaySound(SoundType.WALK);
                _stepTimer = _stepInterval;
            }
        }
        else{
            _stepTimer = 0;
        }
    }
}

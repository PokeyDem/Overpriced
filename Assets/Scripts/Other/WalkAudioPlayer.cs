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

    public void PlayFootstepSfx()
    {
        AudioManager.PlayRandomWalkSound();
    }
}

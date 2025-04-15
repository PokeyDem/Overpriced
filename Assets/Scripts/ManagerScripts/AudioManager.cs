using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonDontDestroyOnLoad<AudioManager>{

    [SerializeField] private AudioClip[] _soundList;
    [SerializeField] private AudioClip[] _stepsList;
    private AudioSource _audioSource;
    private new void Awake(){
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType soundType, float volume = 1){ 
        if (soundType == SoundType.WALK)
            PlayRandomWalkSound();
        else
            Instance._audioSource.PlayOneShot(Instance._soundList[(int)soundType], volume);
    }

    public static void PlayRandomWalkSound(){
        if (Instance._stepsList.Length == 0) return;
        int index = Random.Range(0, Instance._stepsList.Length);
        Instance._audioSource.PlayOneShot(Instance._stepsList[index], 0.1f);
    }
}

public enum SoundType{
    WALK,
    INTERACT
}
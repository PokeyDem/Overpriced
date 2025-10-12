using UnityEngine;

public class AudioManager : SingletonDontDestroyOnLoad<AudioManager>{

    [SerializeField] private AudioClip[] _soundList;
    [SerializeField] private AudioClip[] _stepsList;
    [SerializeField] private AudioClip[] _doorBellSounds;
    [SerializeField] private AudioSource _stepsAudioSource;
    [SerializeField] private AudioSource _defaultAudioSource;
    private new void Awake(){
        base.Awake();
    }

    public static void PlaySound(SoundType soundType, float volume = 1){ 
        if (soundType == SoundType.WALK)
            PlayRandomWalkSound();
        else
            Instance._defaultAudioSource.PlayOneShot(Instance._soundList[(int)soundType], volume);
    }

    public static void PlayRandomWalkSound(){
        if (Instance._stepsList.Length == 0) return;
        int index = Random.Range(0, Instance._stepsList.Length);
        Instance._stepsAudioSource.PlayOneShot(Instance._stepsList[index], 0.1f);
    }

    public static void PlayRandomDoorBellSound(){
        if (Instance._doorBellSounds.Length == 0) return;

        int index = Random.Range(0, Instance._doorBellSounds.Length);
        Instance._defaultAudioSource.PlayOneShot(Instance._doorBellSounds[index], 0.1f);
    }
}

public enum SoundType{
    WALK,
    INTERACT
}
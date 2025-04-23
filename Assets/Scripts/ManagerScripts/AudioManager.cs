using UnityEngine;

public class AudioManager : SingletonDontDestroyOnLoad<AudioManager>{

    [SerializeField] private AudioClip[] _soundList;
    [SerializeField] private AudioClip[] _stepsList;
    [SerializeField] private AudioClip[] _doorBellSounds;
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

    public static void PlayRandomDoorBellSound(){
        if (Instance._doorBellSounds.Length == 0) return;

        int index = Random.Range(0, Instance._doorBellSounds.Length);
        Instance._audioSource.PlayOneShot(Instance._doorBellSounds[index], 0.1f);
    }
}

public enum SoundType{
    WALK,
    INTERACT
}
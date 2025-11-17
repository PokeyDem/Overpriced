using UnityEngine;

public class AudioManager : SingletonDontDestroyOnLoad<AudioManager>{
    
    [SerializeField] private AudioClip[] stepsList;
    [SerializeField] private AudioClip[] doorBellSounds;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip cashRegister;
    [SerializeField] private AudioClip itemPlacement;
    [SerializeField] private AudioSource stepsAudioSource;
    [SerializeField] private AudioSource defaultAudioSource;
    private new void Awake(){
        base.Awake();
    }
    
    public static void PlayRandomWalkSound(){
        if (Instance.stepsList.Length == 0) return;
        int index = Random.Range(0, Instance.stepsList.Length);
        Instance.stepsAudioSource.PlayOneShot(Instance.stepsList[index], 0.1f);
    }

    public static void PlayRandomDoorBellSound(){
        if (Instance.doorBellSounds.Length == 0) return;

        int index = Random.Range(0, Instance.doorBellSounds.Length);
        Instance.defaultAudioSource.PlayOneShot(Instance.doorBellSounds[index], 0.1f);
    }

    public static void PlayButtonClickSfx()
    {
        Instance.defaultAudioSource.PlayOneShot(Instance.buttonClick, 0.25f);
    }

    public static void PlayCashRegisterSfx()
    {
        Instance.defaultAudioSource.PlayOneShot(Instance.cashRegister, 0.1f);
    }
    
    public static void PlayItemPlacementSfx()
    {
        Instance.defaultAudioSource.PlayOneShot(Instance.itemPlacement, 0.1f);
    }
}

public enum SoundType{
    WALK,
    INTERACT
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenuManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetMusicVolume);
            volumeSlider.SetValueWithoutNotify(0.5f);
            SetMusicVolume(0.5f);
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        float dB = (sliderValue > 0.0001f) ? Mathf.Log10(sliderValue) * 20 : -80f;
        audioMixer.SetFloat("MasterVolume", dB);
    }
}

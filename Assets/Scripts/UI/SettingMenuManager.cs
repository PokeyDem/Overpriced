using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenuManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetMusicVolume);
            volumeSlider.SetValueWithoutNotify(0.5f);
            SetMusicVolume(0.5f);
        }
        
        qualityDropdown.ClearOptions();
      
        string[] qualityNames = QualitySettings.names;
     
        List<string> options = new List<string>(qualityNames);
        qualityDropdown.AddOptions(options);
     
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
        
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float dB = (sliderValue > 0.0001f) ? Mathf.Log10(sliderValue) * 20 : -80f;
        audioMixer.SetFloat("MasterVolume", dB);
    }
    
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
     
        UnityEngine.Rendering.OnDemandRendering.renderFrameInterval = 1;
    }
}

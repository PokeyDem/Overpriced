using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireplaceLightDistortion : MonoBehaviour
{
   public float intensityBase = 2.0f;
   public float intensityAmplitude = 1.0f;
   public float flickerSpeed = 2.0f;
   public float colorShiftStrength = 0.1f;

   private Light _fireLight;
   private float _timeOffset;

   private bool _isPlaying;

   void Start()
   {
      if (_fireLight == null)
         _fireLight = GetComponent<Light>();
      _timeOffset = Random.Range(0f, 100f);
   }

   void Update()
   {
      if (_isPlaying)
      {
         float time = Time.time * flickerSpeed + _timeOffset;
      
         float noise = Mathf.PerlinNoise(time, 0.0f);
         _fireLight.intensity = intensityBase + noise * intensityAmplitude;
      }
   }

   public void ChangeLanternsState(bool state)
   {
      _isPlaying = state;
   }
}

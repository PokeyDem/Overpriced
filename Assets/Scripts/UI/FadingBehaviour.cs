using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class FadingBehaviour : SingletonDontDestroyOnLoad<FadingBehaviour>
{
    [SerializeField] private float _duration = 1.0f;
    [SerializeField] private Color32 fadeColor = new Color32(0, 0, 0, 255);
    
    [SerializeField] private Image _mask;
    private bool isFading = false;
    
    private new void Awake()
    {
        base.Awake();
    }

    public void FadeIn(Action onComplete = null){
        
        _mask.raycastTarget = true;
        
        if (!isFading)
        {
            StartCoroutine(FadeRoutine(_mask.color.a * 255, 255, onComplete));
        }
    }
    
    public void FadeOut(Action onComplete = null)
    {
        if (!isFading)
        {
            StartCoroutine(FadeRoutine(_mask.color.a * 255, 0, onComplete));
        }
    }
    
    public void FadeTo(byte targetAlpha)
    {
        if (!isFading)
        {
            StartCoroutine(FadeRoutine(_mask.color.a * 255, targetAlpha));
        }
    }
    
    public void FadeInThenOut(byte targetAlpha = 255)
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutInRoutine(targetAlpha));
        }
    }
    
    private IEnumerator FadeRoutine(float startAlpha, byte targetAlpha, Action onComplete = null)
    {
        isFading = true;
        
        float elapsedTime = 0f;
        Color32 currentColor = _mask.color;
        float startAlphaFloat = startAlpha / 255f;
        float targetAlphaFloat = targetAlpha / 255f;
        
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlphaFloat, targetAlphaFloat, elapsedTime / _duration);
            if (_mask)
                _mask.color = new Color32(currentColor.r, currentColor.g, currentColor.b, (byte)(newAlpha * 255));
            else
                Debug.Log("No fader mask");
            yield return null;
        }
        
        // if (_mask)
        //     _mask.color = new Color32(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        // else
        //     Debug.Log("No fader mask");
        
        if (Mathf.Abs(_mask.color.a - targetAlphaFloat) > 0.001f)
        {
            _mask.color = new Color32(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        }
        
        if (_mask && targetAlpha == 0)
            _mask.raycastTarget = false;
        else
            Debug.Log("No fader mask");
        
        isFading = false;
        
        onComplete?.Invoke();
    }
    
    private IEnumerator FadeOutInRoutine(byte targetAlpha)
    {
        isFading = true;
        
        yield return FadeRoutine(0, targetAlpha);
        
        yield return FadeRoutine((byte)(_mask.color.a * 255), 0);
        
        isFading = false;
    }
    
    public void SetAlpha(byte alpha)
    {
        Color32 currentColor = _mask.color;
        _mask.color = new Color32(currentColor.r, currentColor.g, currentColor.b, alpha);
    }
    
    public bool IsFading()
    {
        return isFading;
    }

   public float GetDuration(){
      return _duration;
   }

   public void SetDuration(float duration){
      _duration = duration;
   }

   public void SetMask(Image newMask)
   {
       _mask = newMask;
   }
}

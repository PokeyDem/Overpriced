using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EndSceneManager : SingletonWithDestroy<EndSceneManager>
{
    [SerializeField] private GameObject endScreenCanvas;
    [SerializeField] private Image backgroundMask;
    [SerializeField] private Image foregroundMask;
    [SerializeField] private float delayBeforeFadeIn;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float amountOfArrows;
    [SerializeField] private float arrowSpawnDelay;
    [SerializeField] private MeshCollider _shopWallsCollider;
    
    private new void Awake()
    {
        base.Awake();
    }

    public void StartEndScene()
    {
        UIManager.Instance.DisableHUD();
        PauseMenuManager.Instance.DisablePausing();
        PlayerControl.Instance.DisableControl();
        
        if (GameManager.Instance.HasWon())
        {
            _shopWallsCollider.enabled = false;
            StartCoroutine(SpawnArrows());
        }
        else
        {
            SceneExplosionBehaviour.Instance.TriggerExplosion();
            StartCoroutine(DelayBeforeFadeIn());
        }
    }

    private void InvokeFadeIn()
    {
        endScreenCanvas.SetActive(true);
        FadingBehaviour.Instance.FadeIn(() => {ShowEndScreenUI();});
    }
    private void ShowEndScreenUI()
    {
        foregroundMask.gameObject.SetActive(true);
            
        UIManager.Instance.ShowEndScreenUIElements(GameManager.Instance.HasWon());
        
        InvokeFadeOut();
    }

    private void InvokeFadeOut()
    {
        FadingBehaviour.Instance.SetMask(foregroundMask);
        FadingBehaviour.Instance.FadeOut();
    }

    private IEnumerator DelayBeforeFadeIn()
    {
        yield return new WaitForSeconds(delayBeforeFadeIn);
        InvokeFadeIn();
    }

    private IEnumerator SpawnArrows()
    {
        for (int i = 0; i < amountOfArrows; i++)
        {
            Instantiate(arrow, new Vector3(arrowSpawnPoint.position.x + Random.Range(-3,3), arrowSpawnPoint.position.y + Random.Range(-3,3), arrowSpawnPoint.position.z + Random.Range(-3,3)), Quaternion.identity);
            yield return new WaitForSeconds(arrowSpawnDelay);
        }
        StartCoroutine(DelayBeforeFadeIn());
    }
}

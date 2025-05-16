using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneManager : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(0);// loading game scene, change when add main menu screen
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

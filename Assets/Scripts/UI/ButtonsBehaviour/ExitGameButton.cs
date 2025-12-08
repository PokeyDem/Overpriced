using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameButton : MonoBehaviour, IInteractableMenuButton
{
    public void Interact()
    {
        MainMenuManager.Instance.OnExitButtonPress(gameObject);
    }
}

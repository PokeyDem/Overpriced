using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameButton : MonoBehaviour, IInteractableMenuButton
{
    public void Interact()
    {
        MainMenuManager.Instance.BlankShot(gameObject);
    }
}

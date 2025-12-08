using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : MonoBehaviour, IInteractableMenuButton
{
    public void Interact()
    {
        MainMenuManager.Instance.OnCreditsButtonPress(gameObject);
    }
}

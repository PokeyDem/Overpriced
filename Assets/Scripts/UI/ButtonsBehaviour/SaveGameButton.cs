using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameButton : MonoBehaviour, IInteractableMenuButton
{
    public void Interact()
    {
        MainMenuManager.Instance.OnSaveGameButtonPress(gameObject);
    }
}

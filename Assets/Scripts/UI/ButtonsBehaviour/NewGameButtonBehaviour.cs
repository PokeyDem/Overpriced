using UnityEngine;

namespace UI
{
  public class NewGameButtonBehaviour : MonoBehaviour, IInteractableMenuButton
  {
    public void Interact()
    {
      MainMenuManager.Instance.OnNewGameButtonPress();
    }
  }
}

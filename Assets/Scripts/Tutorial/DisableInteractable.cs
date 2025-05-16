using System.Collections.Generic;
using UnityEngine;

public class DisableInteractable : MonoBehaviour
{
    [SerializeField] MonoBehaviour monoBehaviour;
    [SerializeField] List<TutorialStateType> stateTypes;

    private void Start()
    {
        if(TutorialManager.Instance != null)
        {
            TutorialManager.Instance.StateMachine.StateChanged += SetCanInteract;
        }
    }
    private void SetCanInteract(BaseTutorialState state)
    {
        if (monoBehaviour is ITutorialable tutorialable)
        {
            bool isActive = true;
            foreach (TutorialStateType tutorialState in stateTypes)
            {
                if (tutorialState == state.Type)
                {
                    isActive = false;
                    break;
                }
            }
            tutorialable.OnTutorialStateChanged(isActive);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOnClickInteraction : MonoBehaviour{
    private IInteractibleOnClick _currentInteractible;
    private IInteractibleOnClick _previousInteractible;
    public UnityEvent MouseInteraction;
    void Update(){
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)){
           _currentInteractible= hit.collider.gameObject.GetComponent<IInteractibleOnClick>();
        }


        if (Input.GetMouseButtonDown(0)){
            if (_currentInteractible != null)
            {
                _currentInteractible.Interact();
                MouseInteraction?.Invoke();
            }

        }
      
            
        if (_currentInteractible != _previousInteractible){

            if (_previousInteractible != null){
                _previousInteractible.ResetInteractionOnHover();
                UIManager.Instance.DisableBuildingInfoPanel();
            }

            if (_currentInteractible != null){
                _currentInteractible.InteractOnHover();
                UIManager.Instance.EnableBuildingInfoPanel();
                UIManager.Instance.SetBuildingInfoPanelPosition(new Vector3(Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    Input.mousePosition.z));
                UIManager.Instance.SetBuildingInfo(_currentInteractible.GetBuildingDescription());
            }
        }
        

        _previousInteractible = _currentInteractible;
    }
}

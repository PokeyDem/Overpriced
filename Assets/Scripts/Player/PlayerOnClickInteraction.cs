using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnClickInteraction : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)){
                IInteractibleOnClick interactible = hit.collider.gameObject.GetComponent<IInteractibleOnClick>();
                if (interactible != null){
                    interactible.Interact();
                }
            }
        }
    }
}

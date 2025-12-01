using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OpenShopButtonController : MonoBehaviour
{
    [SerializeField] private Button button;


    private void OnEnable()
    {
        DisplaysWithItemsListHandler.Instance.listEmpty.AddListener(DisableButton);
        DisplaysWithItemsListHandler.Instance.itemPlaced.AddListener(EnableButton);
        DisplaysWithItemsListHandler.Instance.CheckListEmpty();
    }
    private void OnDisable()
    {
        DisplaysWithItemsListHandler.Instance.listEmpty.RemoveListener(DisableButton);
        DisplaysWithItemsListHandler.Instance.itemPlaced.RemoveListener(EnableButton);
    }
    public void DisableButton()
    {
        button.interactable = false;
    }
    public void EnableButton() 
    {
        button.interactable = true;
    }
}

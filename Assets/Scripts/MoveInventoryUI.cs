using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private RectTransform _inventoryUI;
    [SerializeField] private RectTransform _itemInfoUI;
    private float _screenMiddle = Screen.width / 2f;
    private float _speed=3000;
    private float difference;

    private void Start()
    {
        difference=_inventoryUI.offsetMin.x - _itemInfoUI.offsetMin.x;
    }
    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_player.position);
        //Debug.Log("Player screen position: " + screenPos);
        if (screenPos.x < _screenMiddle)
        {
            MoveUITowards(-185,-22.5f);
        }
        else
        {
            MoveUITowards(-900,11);
        }
    }
    private void MoveUITowards(float target1,float target2)
    {
        Vector2 offset = _inventoryUI.offsetMin;
        Vector2 offset2 = _itemInfoUI.anchoredPosition;
        //_inventoryUI.anchoredPosition = Vector3.MoveTowards(_inventoryUI.anchoredPosition, target, _speed * Time.deltaTime);
        offset.x = Mathf.MoveTowards(offset.x, target1, _speed * Time.deltaTime);
        offset2.x = Mathf.MoveTowards(offset2.x, target2, _speed/2 * Time.deltaTime);
        _inventoryUI.offsetMin = offset;
        _itemInfoUI.anchoredPosition = offset2;
    }
}

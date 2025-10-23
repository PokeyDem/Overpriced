using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisplaySlotController : MonoBehaviour, IDisplayInfoProvider, IDisplayFlagController, IDisplayEditor
{
    [SerializeField] private GameObject _marker;
    [SerializeField] private GameObject priseUI;
    [SerializeField] private GameObject display;
    [SerializeField] private bool isBought;
    [SerializeField] private int prize;
    [SerializeField] private float hiddenAlpha = 0.6f;
    public int Prize => prize;
    public bool IsBought => isBought;

    private Collider _collider;
    private Material[] _materials;
    private int _displayTypeID;
    private float _rotationSpeed = 45f;
    private GameObject _itemPrefab;
    private ItemData _item;
    public bool isOccupied=false;//by npc
    public bool isChosen=false;
    private Vector3 _position;
    public UnityEvent<String> onTextUpdate;


    public Vector3 Position => transform.position;
    public ItemData ItemData => _item;
    public bool IsOccupied { get => isOccupied; set => isOccupied=value; }
    public bool IsChosen { get => isChosen; set => isChosen = value; }


    private void Update(){
        if (_itemPrefab)
            _itemPrefab.transform.Rotate(Vector3.up * (_rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")) {
            if (isBought) {
                _marker.SetActive(true);
            }
            else {
                if (ShopStateManager.ShopStateManagerInstance.ShopIsClose()) {
                    priseUI.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            if (isBought) {
                _marker.SetActive(false);
            }
            else {
                priseUI.SetActive(false);
            }
        }
    }

    private void Awake(){
        _marker.SetActive(false);
        priseUI.SetActive(false);
        onTextUpdate.Invoke(prize.ToString());
    }

    private void Start() {
        if (!isBought) {
            ShopStateManager.ShopStateManagerInstance.shopWosOpen.AddListener(HideDisplay);
            ShopStateManager.ShopStateManagerInstance.shopWosClose.AddListener(ShowToBuy); 
            _materials = display.GetComponent<MeshRenderer>().materials;
            for (int i=0; i<_materials.Length;i++) {
                _materials[i].color = new Color(_materials[i].color.r,_materials[i].color.g,_materials[i].color.b,hiddenAlpha);
            }
            _collider = GetComponent<Collider>();
        }
    }

    public void EnableMarker(){
        if (isBought) {
            _marker.SetActive(true);
        }
        else {
            if (ShopStateManager.ShopStateManagerInstance.ShopIsClose()) {
                priseUI.SetActive(true);
            }
        }
    }

    public void DisableMarker(){
        _marker.SetActive(false);
        priseUI.SetActive(false);
    }

    public void PlaceItem(ItemData item){
        if (!isBought) {
            return;
        }

        if (_itemPrefab != null){
            Destroy(_itemPrefab);
        }
        _itemPrefab = Instantiate(item.Prefab, _marker.transform.position, Quaternion.identity);
        _item = item;
        isChosen = false;
        isOccupied = false;

        IDisplayInfoProvider info = this;
        IDisplayFlagController flags = this;
        IDisplayEditor editor = this;
        DisplaysWithItemsListHandler.Instance.AddDisplaySlotWithItem(new DisplayContext(info,flags,editor));
    }

    public void RemoveItem(){
        Destroy(_itemPrefab);
        _itemPrefab = null;
        _item = null;

        IDisplayInfoProvider info = this;
        IDisplayFlagController flags = this;
        IDisplayEditor editor = this;
        DisplaysWithItemsListHandler.Instance.RemoveDisplaySlotWithItem(new DisplayContext(info, flags,editor));
    }

    public int GetItemId(){
        if (_item==null) {
            return -1;
        }
        return _item.ID;
    }
    //public ItemData GetItem()
    //{
    //    return _item;
    //}

    public void SetDisplayTypeId(int id){
        _displayTypeID = id;
    }

    public int GetDisplayTypeId(){
        return _displayTypeID;
    }

    public void SetPosition(Vector3 position){
        _position = position;
    }

    public void TryToBuy() {
        if (!ShopStateManager.ShopStateManagerInstance.ShopIsClose()) {
            return;
        }
        MoneyManager moneyManager = MoneyManager.Instance;
        if (moneyManager.GetCurrentMoney() >= prize) {
            moneyManager.ReduceMoney(prize);
            isBought = true;
            for (int i=0; i<_materials.Length;i++) {
                _materials[i].color = new Color(_materials[i].color.r,_materials[i].color.g,_materials[i].color.b,1);
            }
            ShopStateManager.ShopStateManagerInstance.shopWosOpen.RemoveListener(HideDisplay);
            ShopStateManager.ShopStateManagerInstance.shopWosClose.RemoveListener(ShowToBuy);
            _collider.enabled = true;
        }
    }

    private void HideDisplay() {
        display.SetActive(false);
        _collider.enabled = false;
    }

    private void ShowToBuy() {
        display.SetActive(true);
        for (int i=0; i<_materials.Length;i++) {
            _materials[i].color = new Color(_materials[i].color.r,_materials[i].color.g,_materials[i].color.b,hiddenAlpha);
        }
        _collider.enabled = true;
    }
}

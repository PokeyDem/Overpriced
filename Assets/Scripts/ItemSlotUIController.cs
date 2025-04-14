using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class ItemSlotUIController : MonoBehaviour {
        private ItemData _item;
        private Image _image;
        private TextMeshProUGUI _quantityDisplay;
        private Image _outline;
        private int _quantity;
        private bool _isOutlineEnabled;
        private TextMeshProUGUI _rarityDisplay;
        
        private void Awake() {
            Initiate();
        }
        
        private void Initiate(){
            _image = gameObject.transform.Find("ItemImage").GetComponent<Image>();
            _outline = gameObject.transform.Find("Outline").GetComponent<Image>();
            _rarityDisplay = gameObject.transform.Find("Rarity").GetComponent<TextMeshProUGUI>();
            _quantityDisplay = GetComponentInChildren<TextMeshProUGUI>();
            DisableOutline();
        }
        public void AddItem(ItemData item){
            if (_item != null && _item != item) {
                throw new Exception("item confilict Exeption");
            }
            if (_item == item) {
                IncreaseQuantity();
                return;
            }

            if (_image==null) {
                Initiate();
            }
            _item = item;
            _quantity=1;
            _image.sprite = item.PreviewImage;
            _quantityDisplay.text = GetItemQuantity().ToString();
            _rarityDisplay.text = new string('*', item.Rarity);
        }
        
        public ItemData RemoveItem() {
            ItemData tmpItem = _item;
            _item = null;
            _image.sprite = null;
            _quantityDisplay.text ="";
            _rarityDisplay.text ="";
            return tmpItem;
        }
        
        public void EnableOutline(){
            Color color = _outline.color;
            color.a = 1f; 
            _outline.color = color;
        }

        public void DisableOutline(){
            Color color = _outline.color;
            color.a = 0f; 
            _outline.color = color;
        }

        public ItemData GetItem() {
            return _item;
        }
        
        public int GetItemID() {
            return _item.ID;
        }
        
        public int GetItemQuantity(){
            return _quantity;
        }
        
        public void IncreaseQuantity(){
            _quantity++;
            _quantityDisplay.text = _quantity.ToString();
        }

        public void DecreaseQuantity(){
            _quantity--;
            _quantityDisplay.text = _quantity.ToString();
        }
        
        public bool IsEmpty(){
            return _item == null;
        }
    }
}
using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


namespace Assets.Scripts.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public static InventoryUI Instance { private set; get; }

        public Transform containerFrame;
        public ItemSlot slotUIPrefab;
        public Inventory inventory;

        private CanvasGroup _canvasGroup;

        private Dictionary<int, ItemSlot> _itemSlots = new Dictionary<int, ItemSlot>();

        private void Awake()
        {
            Instance = this;

            _canvasGroup = GetComponent<CanvasGroup>();
        }


        public void SetUpUI(Ship ship)
        {
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;

            inventory = ship.inventory;
            inventory.OnInventoryChanged += UpdateItemSlots;

        }

        public void Open()
        {
            if (_canvasGroup.interactable)
            {
                Time.timeScale = 1.0f;
                _canvasGroup.alpha = 0.0f;
                _canvasGroup.interactable = false;
            }
            else
            {
                Time.timeScale = 0.0f;
                _canvasGroup.alpha = 1.0f;
                _canvasGroup.interactable = true;
                inventory.OnInventoryChanged.Invoke();
            }
        }

      

        public void UpdateItemSlots()
        {
            for (int i = 0; i < inventory.items.Count; i++)
            {
                CreateItemSlotFromItem(inventory.items[i]);
            }
        }

        private void CreateItemSlotFromItem(Item item)
        {


            if (!_itemSlots.ContainsKey(item.id))
            {
                ItemSlot slot = Instantiate<ItemSlot>(slotUIPrefab, containerFrame);
                slot.title.text = item.Name;
                slot.isEquipped = item.isMounted;
                slot.itemData = item;
                slot.inventory = inventory;

                _itemSlots[item.id] = slot;
            }

        }


    }
}

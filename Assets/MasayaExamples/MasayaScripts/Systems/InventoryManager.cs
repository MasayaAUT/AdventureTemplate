using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasayaScripts.InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {

        public static InventoryManager current; //Reference to the InventoryManager
        public List<ItemData> inventory = new List<ItemData>(); //List of itemdata

        [System.Serializable]
        public class ItemData //A struct containing the item and quantity
        {
            public Item item; //What item it is
            public int quantity; //How many of the item we have

            public void IncreaseQuantity(ItemData item, int amount) //Increases the amount of items
            {
                item.quantity += amount;
            }
            public void RemoveQuantity(ItemData item, int amount) //Removes the amount of items
            {
                item.quantity -= amount;
            }
        }

        private void Awake()
        {
            if (current == null)
            {
                current = this; //Reference itself
            }
        }

        /// <summary>
        /// Adds Item to inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void AddItemToInventory(Item item, int amount)
        {
            foreach(ItemData itemData in inventory) //Checks if item exists
            {
                if(itemData.item == item)
                {
                    itemData.IncreaseQuantity(itemData, amount); //Adds quantity amount to already existing item
                    return;
                }
            }

            //Creates data for the item because it doesn't exist in list
            ItemData newItemData = new ItemData();
            newItemData.item = item; //sets the item
            newItemData.quantity = amount; //sets the amount

            inventory.Add(newItemData); //adds it to list
        }
        /// <summary>
        /// Removes Item from inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void RemoveItemFromInventory(Item item, int amount)
        {
            if(amount > 0) //Checks if we are moving a certain amount
            {
                foreach (ItemData itemData in inventory) //Checks if inventory has the item
                {
                    if (itemData.item == item)
                    {
                        itemData.RemoveQuantity(itemData, amount); //Removes amount from item
                        if (itemData.quantity <= 0)
                        {
                            inventory.Remove(itemData); //Removes item from list if quantity less than zero
                        }
                        return;
                    }
                }
            }
            else
            {
                foreach (ItemData itemData in inventory) //Checks if item exists
                {
                    if (itemData.item == item)
                    {
                        inventory.Remove(itemData);//removes item from inventory
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// Checks if item exists in inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckForItemInInventory(Item item)
        {
            foreach (ItemData itemData in inventory)
            {
                if (itemData.item == item)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if the player is holding x amount of item in inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool CheckItemForQuantity(Item item, int amount)
        {
            foreach (ItemData itemData in inventory)
            {
                if (itemData.item == item && itemData.quantity >= amount)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

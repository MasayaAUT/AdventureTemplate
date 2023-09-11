using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasayaScripts.InventorySystem {
    public class ItemPickup : MonoBehaviour
    {
        public Item item;
        public int amount;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                InventoryManager.current.AddItemToInventory(item, amount);
                Destroy(gameObject);
            }
        }
    }
}

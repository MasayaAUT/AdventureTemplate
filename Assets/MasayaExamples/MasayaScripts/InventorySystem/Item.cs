using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasayaScripts.InventorySystem
{
    [CreateAssetMenu(menuName = "New Item ", fileName = "New Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public ItemType itemType;
        public Sprite itemSprite;

        public enum ItemType
        {
            quest,
            powerups,
            consumable,
            material
        }
    }
}

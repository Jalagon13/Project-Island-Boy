using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class InventoryItem : MonoBehaviour, IInventoryItemInitializer
    {
        [SerializeField] private Image _image;

        public void Initialize(ItemObject item)
        {

        }
    }
}

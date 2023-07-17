using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class ItemPickupSign : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TextMeshProUGUI _itemText;

        private static float _spawnHeight = 2.5f;
        private int _currentStack;
        private string _currentItemName;

        public void Initialize(int amount, string itemName)
        {
            _currentStack = amount;
            _currentItemName = itemName;
            _itemText.text = $"+{_currentStack} {_currentItemName}";

            transform.position = _pr.Position + new Vector2(0f, _spawnHeight);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class WorldItemManager : Singleton<WorldItemManager>
    {
        [SerializeField] private GameObject _itemBasePrefab;
        [SerializeField] private PlayerReference _pr;

        public void SpawnItem(Vector2 worldPos, ItemObject item, int stack = -1, List<ItemParameter> parameterList = null)
        {
            GameObject newItemGo = Instantiate(_itemBasePrefab, worldPos, Quaternion.identity);
            WorldItem newItem = newItemGo.GetComponent<WorldItem>();

            if (stack == -1)
                stack = _pr.PlayerInventory.MaxStack;

            if (!item.Stackable)
                stack = 1;

            newItem.Initialize(item, stack, parameterList);
        }
    }
}

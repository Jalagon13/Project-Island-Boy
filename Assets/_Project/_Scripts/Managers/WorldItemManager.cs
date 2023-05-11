using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class WorldItemManager : Singleton<WorldItemManager>
    {
        [SerializeField] private GameObject _itemBasePrefab;

        public void SpawnItem(Vector2 worldPos, ItemObject item, int stack, List<ItemParameter> parameterList = null)
        {
            GameObject newItemGo = Instantiate(_itemBasePrefab, worldPos, Quaternion.identity);
            WorldItem newItem = newItemGo.GetComponent<WorldItem>();
            newItem.Initialize(item, stack, parameterList);
        }
    }
}

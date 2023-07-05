using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class WorldItemManager : Singleton<WorldItemManager>
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _itemBasePrefab;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private ItemObject _testItem;

        public void SpawnTestItem()
        {
            SpawnItem(new Vector2(6, 0), _testItem, 10);
        }

        public void SpawnItem(Vector2 worldPos, ItemObject item, int stack = -1, List<ItemParameter> parameterList = null)
        {
            AudioManager.Instance.PlayClip(_popSound, false, true, 1);

            GameObject newItemGo = Instantiate(_itemBasePrefab, worldPos, Quaternion.identity);
            WorldItem newItem = newItemGo.GetComponent<WorldItem>();

            if (stack < 0)
                stack = _pr.Inventory.MaxStack;

            if (!item.Stackable)
                stack = 1;

            newItem.Initialize(item, stack, parameterList);
        }
    }
}

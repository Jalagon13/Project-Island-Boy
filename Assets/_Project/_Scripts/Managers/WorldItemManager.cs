using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class WorldItemManager : Singleton<WorldItemManager>
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _itemBasePrefab;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private ItemObject _testItem1;
        [SerializeField] private ItemObject _testItem2;

        public void SpawnTestItem()
        {
            SpawnItem(new Vector2(4, 0), _testItem1, 5);
            SpawnItem(new Vector2(5, 0), _testItem2, 4);
        }

        public GameObject SpawnItem(Vector2 worldPos, ItemObject item, int stack = -1, List<ItemParameter> parameterList = null, bool playAudio = true)
        {
            GameObject newItemGo = Instantiate(_itemBasePrefab, worldPos, Quaternion.identity);
            WorldItem newItem = newItemGo.GetComponent<WorldItem>();

            if (playAudio)
                AudioManager.Instance.PlayClip(_popSound, false, true, 1);

            if (stack < 0)
                stack = _pr.Inventory.MaxStack;

            if (!item.Stackable)
                stack = 1;

            newItem.Initialize(item, stack, parameterList);

            return newItemGo;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class WorldItemManager : Singleton<WorldItemManager>
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private GameObject _itemBasePrefab;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private List<ItemObject> _startingItems;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            foreach (ItemObject item in _startingItems)
            {
                SpawnItem(_pr.Position, item);
            }
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

            if(item.DefaultParameterList.Count > 0)
            {
                newItem.Initialize(item, stack, item.DefaultParameterList);
            }
            else
            {
                newItem.Initialize(item, stack, parameterList);
            }

            return newItemGo;
        }
    }
}

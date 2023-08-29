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
        [SerializeField] private ItemObject _startingHooks;
        [SerializeField] private GameObject _itemBasePrefab;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private LootTable _seaLoot;

        protected override void Awake()
        {
            base.Awake();
            //SpawnItem(new Vector2(-0.5f, 0f), _startingHooks, 1, null, false);
        }

        private IEnumerator Start()
        {
            foreach (KeyValuePair<ItemObject, int> loot in _seaLoot.Loot())
            {
                yield return new WaitForSeconds(Random.Range(1f, 5f));

                if (SceneManager.GetActiveScene().buildIndex != 0)
                    break;

                float randomX = Random.value;
                float randomY = Random.value;

                if (Random.value < 0.5f)
                    randomX = randomX < 0.5f ? -0.1f : 1.1f;
                else
                    randomY = randomY < 0.5f ? -0.1f : 1.1f;

                Vector2 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector2(randomX, randomY));

                var itemGo = SpawnItem(spawnPosition, loot.Key, loot.Value, null, false);
                itemGo.AddComponent<ItemSeaWander>().StartWander(_tmr.GroundTilemap);
            }

            StartCoroutine(Start());
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

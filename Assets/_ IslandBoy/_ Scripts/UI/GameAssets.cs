using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class GameAssets : MonoBehaviour
    {
        public Transform pfDamagePopup;

        [SerializeField] private GameObject _itemBasePrefab;
        [SerializeField] private AudioClip _popSound;

        private static GameAssets _i;

        public static GameAssets Instance
        {
            get
            {
                if (_i == null)
                    _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
                return _i;
            }
        }

        public GameObject SpawnItem(Vector2 worldPos, ItemObject item, int stack, List<ItemParameter> parameterList = null, bool playAudio = true)
        {
            GameObject newItemGo = Instantiate(_itemBasePrefab, worldPos, Quaternion.identity);
            WorldItem newItem = newItemGo.GetComponent<WorldItem>();

            if (playAudio)
                MMSoundManagerSoundPlayEvent.Trigger(_popSound, MMSoundManager.MMSoundManagerTracks.UI, default);

            if (!item.Stackable || stack <= 0)
                stack = 1;

            if (item.DefaultParameterList.Count > 0)
                newItem.Initialize(item, stack, item.DefaultParameterList);
            else
                newItem.Initialize(item, stack, parameterList);

            return newItemGo;
        }
    }
}

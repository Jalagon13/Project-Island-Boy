using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class HousingControl : MonoBehaviour
    {
        [SerializeField] private GameObject _npcSlotPrefab;
        [SerializeField] private List<NpcObject> _npcs = new();

        private RectTransform _npcSlotHolder;

        private void Awake()
        {
            _npcSlotHolder = transform.GetChild(0).GetComponent<RectTransform>();
        }

        private void Start()
        {
            _npcs.ForEach(npc =>
            {
                GameObject ns = Instantiate(_npcSlotPrefab, _npcSlotHolder.transform);

                NpcSlot npcSlot = ns.GetComponent<NpcSlot>();
                npcSlot.Initialize(npc);
            });
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class HousingControl : MonoBehaviour
    {
        [SerializeField] private GameObject _npcSlotPrefab;
        [SerializeField] private RectTransform _npcSlotHolder;
        [SerializeField] private List<NpcObject> npcsFound = new();

        private void OnEnable()
        {
            GameSignals.DAY_END.AddListener(UpdateNpcs);
        }

        private void OnDisable()
        {
            GameSignals.DAY_END.RemoveListener(UpdateNpcs);
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            //foreach (NpcObject npc in npcsFound)
            //{
            //    npc.MoveOut();
            //}

            //ClearNpcHolder();
            //CheckBeds();
            UpdateNpcSlots();
        }

        private void ClearNpcHolder()
        {
            if (_npcSlotHolder.transform.childCount > 0)
            {
                foreach (Transform child in _npcSlotHolder.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void CheckBeds()
        {
            Bed[] beds = FindObjectsOfType<Bed>();
            List<Bed> bedsInValidHouse = new();

            // loop through all beds and find the ones that have a in a valid house.
            foreach (Bed bed in beds)
            {
                if (bed.InValidSpace())
                    bedsInValidHouse.Add(bed);
                else if (bed.NPC != null)
                {
                    bed.NPC.MoveOut();
                    bed.NPC = null;
                }
            }
            // loop through all valid beds and find unOccupied ones
            Stack<Bed> unOccupiedValidBeds = new();

            foreach (Bed bed1 in bedsInValidHouse)
            {
                if(bed1.NPC == null)
                    unOccupiedValidBeds.Push(bed1);
            }
            // loop through all non-moved in npcs and add as many as you can to each bed
            foreach (NpcObject npc in npcsFound)
            {
                if (!npc.MovedIn)
                {
                    if(unOccupiedValidBeds.Count <= 0)
                        continue;

                    Bed bedToMoveIn = unOccupiedValidBeds.Pop();
                    npc.MoveIn(bedToMoveIn);
                    bedToMoveIn.NPC = npc;
                }
            }

            //loop through all moved-in npcs and move out all NPCs without a bed.
            foreach (NpcObject npc1 in npcsFound)
            {
                if (npc1.MovedIn)
                {
                    if (npc1.Bed == null)
                        npc1.MoveOut();
                }
            }
        }

        private void UpdateNpcs(ISignalParameters parameters)
        {
            
            ClearNpcHolder();
            CheckBeds();
            UpdateNpcSlots();
        }

        private void UpdateNpcSlots()
        {
            foreach (NpcObject npc in npcsFound)
            {
                var go = Instantiate(_npcSlotPrefab, _npcSlotHolder);
                var npcSlot = go.GetComponent<NpcSlot>();
                npcSlot.Initialize(npc);
            }
        }
    }
}

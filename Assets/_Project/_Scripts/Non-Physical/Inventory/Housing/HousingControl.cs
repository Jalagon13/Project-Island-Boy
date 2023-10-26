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
        [SerializeField] private List<NpcObject> _npcs = new();

        private void OnEnable()
        {
            DayManager.Instance.OnEndDay += UpdateNpcs;
        }

        private void OnDisable()
        {
            DayManager.Instance.OnEndDay -= UpdateNpcs;
        }

        private void Start()
        {
            CheckBeds();
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

            foreach (NpcObject npc in _npcs)
            {
                bool npcFoundBed = false;

                // look through all the beds that are unoccupied
                foreach (Bed bed in beds)
                {
                    // if a bed is unoccupied, try to occupy it.
                    if(bed.NpcClaimed == null)
                    {
                        npc.MovedIn = bed.FindFurniture(npc);

                        if (npc.MovedIn)
                        {
                            npc.MoveInNpc(bed);
                            GameObject ns = Instantiate(_npcSlotPrefab, _npcSlotHolder.transform);
                            NpcSlot npcSlot = ns.GetComponent<NpcSlot>();
                            npcSlot.Initialize(npc);
                            npcFoundBed = true;
                        }
                    }
                }

                if (npcFoundBed)
                    continue;

                // look through all the beds that are occupied
                foreach (Bed bed in beds)
                {
                    // if a bed is occupied, check if the occupier can still occuppy it
                    if (bed.NpcClaimed != null && bed.NpcClaimed != npc)
                    {
                        bool occupierCanOccppy = bed.FindFurniture(bed.NpcClaimed);

                        // if the occupier can occupy, continue to next occupied bed
                        if (occupierCanOccppy)
                            continue;
                        else
                        {
                            // if not, move them out
                            bed.NpcClaimed.MoveOutNpc();
                            bed.NpcClaimed = null;

                            // try to move into this bed
                            npc.MovedIn = bed.FindFurniture(npc);

                            if (npc.MovedIn)
                            {
                                npc.MoveInNpc(bed);
                                GameObject ns = Instantiate(_npcSlotPrefab, _npcSlotHolder.transform);
                                NpcSlot npcSlot = ns.GetComponent<NpcSlot>();
                                npcSlot.Initialize(npc);
                                npcFoundBed = true;
                            }
                        }
                    }
                }

                if (npcFoundBed)
                    continue;

                // look through all the beds that are occupied by this npc
                foreach (Bed bed in beds)
                {
                    if(bed.NpcClaimed == npc)
                    {
                        // check if npc can still move in to this
                        npc.MovedIn = bed.FindFurniture(npc);

                        if (npc.MovedIn)
                        {
                            npc.MoveInNpc(bed);
                            GameObject ns = Instantiate(_npcSlotPrefab, _npcSlotHolder.transform);
                            NpcSlot npcSlot = ns.GetComponent<NpcSlot>();
                            npcSlot.Initialize(npc);
                            npcFoundBed = true;
                        }
                    }
                }

                if (npcFoundBed)
                    continue;

                npc.MoveOutNpc();
            }
        }

        private void UpdateNpcs(object sender, EventArgs e)
        {
            ClearNpcHolder();
            CheckBeds();
        }
    }
}

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
            _npcs.ForEach(npc =>
            {
                GameObject ns = Instantiate(_npcSlotPrefab, _npcSlotHolder.transform);

                NpcSlot npcSlot = ns.GetComponent<NpcSlot>();
                npcSlot.Initialize(npc);
            });
        }

        private void UpdateNpcs(object sender, EventArgs e)
        {
            Bed[] beds = FindObjectsOfType<Bed>();

            foreach (NpcObject npc in _npcs)
            {
                bool foundValidHousing = false;
                Debug.Log(npc.Name + " " + foundValidHousing);

                // loop through all beds in the world to try to find a valid housing
                foreach (Bed bed in beds)
                {
                    foundValidHousing = bed.FindFurniture(npc);

                    if (foundValidHousing)
                    {
                        // update npc for able to move in.
                        Debug.Log($"{npc.Name} Can move in!");
                        break;
                    }
                }

                if (!foundValidHousing)
                {
                    // if not, update the npc to not moved in status and don't move it in.
                    Debug.Log($"{npc.Name} Can NOT move in!");
                }
            }
        }
    }
}

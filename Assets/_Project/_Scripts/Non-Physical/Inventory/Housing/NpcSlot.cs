using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class NpcSlot : MonoBehaviour
    {
        public void Initialize(NpcObject npc)
        {
            var hover = transform.GetChild(0).GetComponent<HousingHoverImage>();
            var image = transform.GetChild(0).GetComponent<Image>();
            var description = npc.MovedIn ? $"{npc.Description}<br>Status: <color=green>Moved in!" : $"{npc.Description}<br>Status: <color=red>Not moved in!";

            hover.Initialize(npc.Name, description);
            image.sprite = npc.Icon;
        }
    }
}

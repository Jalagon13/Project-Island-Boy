using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public static class GameSignals
    {
        // DayManager Signals
        public static readonly Signal DAY_START = new("DayStart");
        public static readonly Signal DAY_END = new("DayEnd");
        public static readonly Signal DAY_OUT_OF_TIME = new("DayOutOfTime");
        public static readonly Signal NPC_MOVED_IN = new("NpcMovedIn");
        public static readonly Signal NPC_MOVED_OUT = new("NpcMovedOut");
        public static readonly Signal CAN_SLEEP = new("CanSleep");

        // Player Signals
        public static readonly Signal INVENTORY_OPEN = new("InventoryOpen");
        public static readonly Signal INVENTORY_CLOSE = new("InventoryClose");
        public static readonly Signal SELECTED_SLOT_UPDATED = new("SelectedSlotUpdated");
        public static readonly Signal SLOT_CLICKED = new("SlotClicked");
        public static readonly Signal ITEM_ADDED = new("ItemAdded");
        public static readonly Signal ITEM_CRAFTED = new("ItemCrafted");
        public static readonly Signal CHEST_INTERACT = new("ChestInteract");
    }
}

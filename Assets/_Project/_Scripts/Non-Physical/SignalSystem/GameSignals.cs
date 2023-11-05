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
        public static readonly Signal CRAFT_STATION_INTERACT = new("CraftStationInteract");
        public static readonly Signal CONSUME_ITEM_TRY = new("ConsumeItemTry");
        public static readonly Signal CONSUME_ITEM_SUCCESS = new("ConsumeItemSuccess");
        public static readonly Signal SWING_PERFORMED = new("SwingPerformed");
        public static readonly Signal OBJECT_LAUNCHED = new("ObjectLaunched");
        public static readonly Signal OBJECT_PLACED = new("ObjectPlaced");
        public static readonly Signal PLAYER_DIED = new("PlayerDied");
        public static readonly Signal PROMPT_INTERACT = new("PromptInteract");

        // Pause Signals
        public static readonly Signal GAME_PAUSED = new("GamePaused");
        public static readonly Signal GAME_UNPAUSED = new("GameUnpaused");
    }
}

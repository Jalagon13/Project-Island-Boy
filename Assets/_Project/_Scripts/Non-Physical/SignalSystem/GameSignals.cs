using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public static class GameSignals
    {
        public static readonly Signal DAY_STARTED = new("DayStarted");
        public static readonly Signal DAY_ENDED = new("DayEnded");
        public static readonly Signal NPC_MOVED_IN = new("NpcMovedIn");
        public static readonly Signal NPC_MOVED_OUT = new("NpcMovedOut");
    }
}

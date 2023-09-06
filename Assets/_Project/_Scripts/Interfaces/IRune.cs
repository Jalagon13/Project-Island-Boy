using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public interface IRune
    {
        public void Initialize(TileAction ta, List<IRune> runeList);
        public void Execute();
    }
}

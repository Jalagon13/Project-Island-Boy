using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public interface IRune
    {
        public void Execute(TileAction ta);
    }
}

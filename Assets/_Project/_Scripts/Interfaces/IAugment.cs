using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public interface IAugment
    {
        public void Execute(TileAction ta);
    }
}

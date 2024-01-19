using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Forageable : Resource
    {
        [SerializeField] protected bool _naturallySpawns = false;
        [SerializeField] protected int _id;

        public int ID()
        {
            return _id;
        }

        public bool SpawnsNaturally()
        {
            return _naturallySpawns;
        }
    }
}

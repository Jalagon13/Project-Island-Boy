using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Augment : MonoBehaviour, IAugment
    {
        public void Execute(TileAction ta)
        {
            Debug.Log($"TA Pos: {ta.gameObject.transform.position}");
            Debug.Log("PEW PEW PEW");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Furniture : Clickable
    {
        public override void ShowDisplay()
        {
            _instructions.gameObject.SetActive(true);
        }
    }
}

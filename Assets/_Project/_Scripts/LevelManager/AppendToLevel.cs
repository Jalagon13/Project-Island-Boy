using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class AppendToLevel : MonoBehaviour
    {
        public static event Action<GameObject> AppendToLevelEvent;

        private void Start()
        {
            AppendToLevelEvent?.Invoke(gameObject);
        }
    }
}

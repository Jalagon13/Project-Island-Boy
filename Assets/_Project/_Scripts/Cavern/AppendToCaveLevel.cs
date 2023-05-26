using System;
using UnityEngine;

namespace IslandBoy
{
    public class AppendToCaveLevel : MonoBehaviour
    {
        public static event Action<GameObject> AppendtoCaveEvent;

        private void Start()
        {
            AppendtoCaveEvent?.Invoke(gameObject);
        }
    }
}

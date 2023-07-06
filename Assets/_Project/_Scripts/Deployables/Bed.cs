using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Bed : MonoBehaviour
    {
        private void Start()
        {
            IslandManager.Instance.PushBedPosition(transform.position);
        }

        private void OnDestroy()
        {
            IslandManager.Instance.PopBedPosition(transform.position);
        }

        public void EndDay()
        {
            DayManager.Instance.EndDay();
        }
    }
}

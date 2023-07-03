using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Bed : MonoBehaviour
    {
        private void Start()
        {
            DayManager.Instance.PushBedPosition(transform.position);
        }

        private void OnDestroy()
        {
            DayManager.Instance.PopBedPosition(transform.position);
        }

        public void EndDay()
        {
            DayManager.Instance.EndDay();
        }
    }
}

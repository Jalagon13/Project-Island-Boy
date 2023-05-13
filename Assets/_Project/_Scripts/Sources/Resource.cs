using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Resource : MonoBehaviour, IBreakable
    {
        [SerializeField] private int _hitPoints;

        public int HitPoints { get { return _hitPoints; } set { _hitPoints = value; } }

        public void Hit()
        {
            Debug.Log($"{gameObject.name} was hit!");
        }

        public void Break()
        {
            
        }
    }
}

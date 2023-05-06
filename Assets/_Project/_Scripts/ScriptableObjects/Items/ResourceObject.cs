using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Create Item/New Resource")]
    public class ResourceObject : ItemObject
    {
        [Header("Resource Parameters")]
        [SerializeField] private bool _stackable = true;
        public bool Stackable { get { return _stackable; } }
    }
}

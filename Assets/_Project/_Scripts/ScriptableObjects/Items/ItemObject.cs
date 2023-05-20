using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public abstract class ItemObject : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite UiDisplay { get; private set; }
        [field: SerializeField] public bool Stackable { get; private set; }
        [field: TextArea]
        [field: SerializeField] public string Description { get; set; }
        [field: SerializeField] public List<ItemParameter> DefaultParameterList { get; set; }

        public abstract ToolType ToolType { get; }
        public abstract void ExecuteAction(SelectedSlotControl control);

    }

    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterObject Parameter;
        public float Value;

        public bool Equals(ItemParameter other)
        {
            return other.Parameter == Parameter;
        }
    }
}

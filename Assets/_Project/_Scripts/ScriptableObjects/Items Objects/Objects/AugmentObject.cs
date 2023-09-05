using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Augment", menuName = "Create Item/New Augment")]
    public class AugmentObject : ItemObject
    {
        [field: SerializeField] public GameObject Augment { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public List<ToolType> CombatableToolTypes { get; private set; }

        public override ToolType ToolType => _baseToolType;

        public override AmmoType AmmoType => _baseAmmoType;

        public override ArmorType ArmorType => _baseArmorType;

        public override GameObject AmmoPrefab => null;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            
        }

        public override string GetDescription()
        {
            return Description;
        }
    }
}

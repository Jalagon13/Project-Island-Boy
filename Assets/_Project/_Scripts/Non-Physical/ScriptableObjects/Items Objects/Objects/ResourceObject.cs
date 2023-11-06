using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Create Item/New Resource")]
    public class ResourceObject : ItemObject
    {
        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => 0;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {

        }

        public override string GetDescription()
        {
            return $"{Description}";
        }
    }
}

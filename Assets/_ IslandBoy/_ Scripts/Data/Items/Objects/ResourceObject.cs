using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Create Item/New Resource")]
    public class ResourceObject : ItemObject
    {
        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {

        }

        public override string GetDescription()
        {
            return $"* Material{Description}";
        }
    }
}

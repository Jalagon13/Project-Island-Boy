using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Create Item/New Resource")]
    public class ResourceObject : ItemObject
    {
        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            
        }

        public override string GetDescription()
        {
            return $"{Description}";
        }
    }
}

using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Create Item/New Resource")]
    public class ResourceObject : ItemObject
    {
        public override ToolType ToolType => ToolType.Ax;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            
        }
    }
}

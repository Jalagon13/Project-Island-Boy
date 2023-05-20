using System.Text;
using UnityEngine;

namespace IslandBoy
{
    public enum ToolType
    {
        Indifferent,
        Ax,
        Pickaxe,
        Sword
    }

    [CreateAssetMenu(fileName = "New Tool", menuName = "Create Item/New Tool")]
    public class ToolObject : ItemObject
    {
        [field: SerializeField] public ToolType Type { get; set; }

        public override ToolType ToolType => Type;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            
        }

        public override string GetDescription()
        {
            string powerDesc = string.Empty;
            string cooldownDesc = string.Empty;

            foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    case "Power":
                        powerDesc = $"{item.Value} {ToolType} Power<br>";
                        break;
                    case "Cooldown":
                        cooldownDesc = $"{item.Value}s Cooldown<br>";
                        break;
                }
            }

            return $"{powerDesc}{cooldownDesc}";
        }
    }
}

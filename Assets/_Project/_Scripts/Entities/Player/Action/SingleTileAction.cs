using UnityEngine;

namespace IslandBoy
{
    public class SingleTileAction : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private ItemParameter _powerParameter;
        [SerializeField] private ItemParameter _durabilityParameter;

        private float _basePower;
        private ToolType _baseToolType;

        public float BasePower { set { _basePower = value; } }
        public ToolType BaseToolType { set { _baseToolType = value; } }

        private void Update()
        {
            transform.position = CalcStaPos();
        }

        public bool IsClear()
        {
            // need to flesh this out later
            // checks if there is empty space over the STA
            return true;
        }

        public void HitTile()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

            foreach (var collider in colliders)
            {
                IBreakable breakable = collider.GetComponent<IBreakable>();

                if (breakable != null)
                {
                    if (breakable.Hit(CalcPower(), CalcToolType()))
                        ModifyDurability();
                }
            }
        }

        private void ModifyDurability()
        {
            if (_pr.SelectedSlot.CurrentParameters.Count <= 0) return;

            var itemParams = _pr.SelectedSlot.CurrentParameters;

            if (itemParams.Contains(_durabilityParameter))
            {
                int index = itemParams.IndexOf(_durabilityParameter);
                float newValue = itemParams[index].Value - 1;
                itemParams[index] = new ItemParameter
                {
                    Parameter = _durabilityParameter.Parameter,
                    Value = newValue
                };

                _pr.SelectedSlot.InventoryItem.UpdateDurabilityCounter();
            }
        }

        private float CalcPower()
        {
            if (_pr.SelectedSlot.CurrentParameters.Count <= 0) return _basePower;

            var itemParams = _pr.SelectedSlot.CurrentParameters;

            if (itemParams.Contains(_powerParameter))
            {
                int index = itemParams.IndexOf(_powerParameter);
                return itemParams[index].Value;
            }
            
            return _basePower;
        }

        private ToolType CalcToolType()
        {
            var item = _pr.SelectedSlot.ItemObject;
            return item == null ? _baseToolType : item.ToolType;
        }

        private Vector2 CalcStaPos()
        {
            var playerPosTileCenter = GetCenterOfTilePos(_pr.PositionReference);
            var dir = (_pr.MousePositionReference - playerPosTileCenter).normalized;

            return GetCenterOfTilePos(playerPosTileCenter + dir);
        }

        private Vector2 GetCenterOfTilePos(Vector3 pos)
        {
            var xPos = Mathf.FloorToInt(pos.x) + 0.5f;
            var yPos = Mathf.FloorToInt(pos.y) + 0.5f;

            return new Vector2(xPos, yPos);
        }
    }
}

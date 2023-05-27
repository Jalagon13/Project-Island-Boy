using UnityEngine;

namespace IslandBoy
{
    public class SingleTileAction : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private ItemParameter _powerParameter;
        [SerializeField] private ItemParameter _durabilityParameter;

        private SingleTileHpCanvas _stHpCanvas;
        private SingleTileIndicator _stIndicator;
        private ToolType _baseToolType;
        private float _basePower;

        public float BasePower { set { _basePower = value; } }
        public ToolType BaseToolType { set { _baseToolType = value; } }

        private void Awake()
        {
            _stHpCanvas = GetComponent<SingleTileHpCanvas>();
            _stIndicator = GetComponent<SingleTileIndicator>();
        }

        private void Update()
        {
            transform.position = CalcStaPos();
        }

        public bool IsClear()
        {
            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.5f), 0);

            foreach(Collider2D col in colliders)
            {
                if(col.gameObject.layer == 3) 
                    return false;
            }

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
                    {
                        ModifyDurability();

                        if (breakable.CurrentHitPoints <= 0)
                        {
                            _stHpCanvas.HideHpCanvas();
                            _stIndicator.ChangeToEmpty();
                        }
                        else
                        {
                            _stHpCanvas.ShowHpCanvas(breakable.MaxHitPoints, breakable.CurrentHitPoints);
                        }
                    }
                }
            }
        }

        public void PlaceDeployable(GameObject deployable)
        {
            var position = transform.position -= new Vector3(0.5f, 0.5f);
            Instantiate(deployable, position, Quaternion.identity);
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
            var playerPosTileCenter = GetCenterOfTilePos(_pr.PositionReference + new Vector2(0f, 0.4f));
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

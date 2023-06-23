using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Throw Object", menuName = "Create Item/New Throw Object")]
    public class ThrowObject : ItemObject
    {
        [SerializeField] private GameObject _prefabToThrow;

        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            GameObject throwObject = Instantiate(_prefabToThrow, (Vector3)control.PR.PositionReference, Quaternion.identity);

            if(throwObject.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(Vector2.right * 15, ForceMode2D.Impulse);
            }

            control.PR.SelectedSlot.InventoryItem.Count--;
        }

        public override string GetDescription()
        {
            return $"• Can be thrown<br>{Description}";
        }
    }
}

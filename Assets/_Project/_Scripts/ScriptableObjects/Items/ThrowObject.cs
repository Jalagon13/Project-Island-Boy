using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Throw Object", menuName = "Create Item/New Throw Object")]
    public class ThrowObject : ItemObject
    {
        [SerializeField] private GameObject _prefabToThrow;
        [SerializeField] private AudioClip _throwSound;
        [SerializeField] private float _throwForce = 15f;

        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            GameObject throwObject = Instantiate(_prefabToThrow, (Vector3)control.PR.PositionReference + new Vector3(0, 0.4f), Quaternion.identity);

            if(throwObject.TryGetComponent(out Rigidbody2D rb))
            {
                Vector2 direction = ((Vector3)control.PR.MousePositionReference - rb.transform.position).normalized;

                AudioManager.Instance.PlayClip(_throwSound, false, true);

                rb.AddForce(direction * _throwForce, ForceMode2D.Impulse);
            }

            control.PR.SelectedSlot.InventoryItem.Count--;
        }

        public override string GetDescription()
        {
            return $"• Can be thrown<br>{Description}";
        }
    }
}

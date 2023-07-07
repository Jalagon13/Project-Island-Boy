using System;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Throw Object", menuName = "Create Item/New Throw Object")]
    public class ThrowObject : ItemObject
    {
        public static event Action ThrowEvent;

        [SerializeField] private GameObject _prefabToThrow;
        [SerializeField] private AudioClip _throwSound;

        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            control.IsCharging = true;
            control.OnThrow = Throw;
        }

        private void Throw(SelectedSlotControl control, float force)
        {
            GameObject throwObject = Instantiate(_prefabToThrow, (Vector3)control.PR.Position + new Vector3(0, 0.4f), Quaternion.identity);

            if (throwObject.TryGetComponent(out Rigidbody2D rb))
            {
                Vector2 direction = ((Vector3)control.PR.MousePosition - rb.transform.position).normalized;

                AudioManager.Instance.PlayClip(_throwSound, false, true);

                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }

            ThrowEvent?.Invoke();
            control.PR.SelectedSlot.InventoryItem.Count--;
        }

        public override string GetDescription()
        {
            return $"• Can be thrown<br>{Description}";
        }
    }
}

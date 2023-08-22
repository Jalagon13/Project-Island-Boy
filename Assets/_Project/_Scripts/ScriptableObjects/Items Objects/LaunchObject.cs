using System;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Launch Object", menuName = "Create Item/New Launch Object")]
    public class LaunchObject : ItemObject
    {
        public static event Action LaunchEvent;

        [SerializeField] private GameObject _prefabToThrow;
        [SerializeField] private AudioClip _throwSound;
        [Header("Optional force added or subtracted")]
        [SerializeField] private float _launchForce;

        public override ToolType ToolType => _baseToolType;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5)) return;

            control.IsCharging = true;
            control.OnLaunch = Launch;
        }

        private void Launch(SelectedSlotControl control, float force)
        {
            GameObject throwObject = Instantiate(_prefabToThrow, (Vector3)control.PR.Position + new Vector3(0, 0.4f), Quaternion.identity);

            if (throwObject.TryGetComponent(out Rigidbody2D rb))
            {
                Vector2 direction = ((Vector3)control.PR.MousePosition - rb.transform.position).normalized;

                AudioManager.Instance.PlayClip(_throwSound, false, true);

                var launchForce = (direction * (force + _launchForce));

                rb.AddForce(launchForce, ForceMode2D.Impulse);
            }

            LaunchEvent?.Invoke();
            
            if (Stackable)
            {
                control.PR.SelectedSlot.InventoryItem.Count--;
            }
            else
            {
                control.TileAction.ModifyDurability();
            }
        }

        public override string GetDescription()
        {
            return $"• Can be launched<br>{Description}";
        }
    }
}

using System;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Launch Object", menuName = "Create Item/New Launch Object")]
    public class LaunchObject : ItemObject
    {
        public static event Action LaunchEvent;

        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _throwPrefab;
        [SerializeField] private AudioClip _throwSound;
        [SerializeField] private float _launchForce = 5f; // Optional force added or subtracted
        [SerializeField] private AmmoType _ammoType;

        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => 0;

        private ItemObject _ammoObject;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5) || control.TileAction.OverInteractable()) return;

            _ammoObject = null;

            if (_ammoType != AmmoType.None)
            {
                ItemObject ammoObject = _pr.Inventory.GetAmmoItem(_ammoType);

                if (ammoObject == null) return;

                _ammoObject = ammoObject;
            }

            control.IsCharging = true;
            control.OnLaunch = Launch;
        }

        private void Launch(SelectedSlotControl control, float force)
        {
            LaunchProjectile(control, force);
            AfterLaunch(control);

            AudioManager.Instance.PlayClip(_throwSound, false, true);
            LaunchEvent?.Invoke();
        }

        private void AfterLaunch(SelectedSlotControl control)
        {
            if (_ammoObject != null)
            {
                _pr.Inventory.RemoveItem(_ammoObject, 1);
                _ammoObject = null;
            }

            if (Stackable)
                control.PR.SelectedSlot.InventoryItem.Count--;
            else
                control.TileAction.ModifyDurability();
        }

        private void LaunchProjectile(SelectedSlotControl control, float force)
        {
            Vector2 launchPosition = (Vector3)control.PR.Position + new Vector3(0, 0.4f);

            GameObject launchPrefab = _ammoObject == null ? _throwPrefab : _ammoObject.AmmoPrefab;
            GameObject launchObject = Instantiate(launchPrefab, launchPosition, Quaternion.identity);
            Rigidbody2D rb = launchObject.GetComponent<Rigidbody2D>();

            Vector2 direction = ((Vector3)control.PR.MousePosition - rb.transform.position).normalized;
            Vector2 launchForce = (direction * (force + _launchForce));

            if (launchObject.TryGetComponent(out Ammo arrow))
            {
                arrow.Setup(this, _ammoObject, control.ThrowForcePercentage);
            }

            rb.AddForce(launchForce, ForceMode2D.Impulse);
        }

        public override string GetDescription()
        {
            return $"• Can be launched<br>{Description}";
        }
    }
}

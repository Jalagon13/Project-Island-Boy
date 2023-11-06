using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Launch Object", menuName = "Create Item/New Launch Object")]
    public class LaunchObject : ItemObject
    {
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
        private SelectedSlotControl _ssc;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {

        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5) || control.TileAction.OverInteractable()) return;

            _ammoObject = null;
            _ssc = control;

            if (_ammoType != AmmoType.None)
            {
                ItemObject ammoObject = _pr.Inventory.GetAmmoItem(_ammoType);

                if (ammoObject == null) return;

                _ammoObject = ammoObject;
            }

            DispatchItemCharging();
        }

        private void DispatchItemCharging()
        {
            Action<float, float> behavior = LaunchReleaseBehavior;
            Signal signal = GameSignals.ITEM_CHARGING;
            signal.ClearParameters();
            signal.AddParameter("ReleaseBehavior", behavior);
            signal.Dispatch();
        }

        public void LaunchReleaseBehavior(float chargePercentage, float baseLaunchForce)
        {
            LaunchProjectile(chargePercentage, baseLaunchForce);
            AfterLaunch();
        }

        private void AfterLaunch()
        {
            if (_ammoObject != null)
            {
                _pr.Inventory.RemoveItem(_ammoObject, 1);
                _ammoObject = null;
            }

            if (Stackable)
                _ssc.SelectedSlot.InventoryItem.Count--;
        }

        private void LaunchProjectile(float launchForcePercentage, float baseLaunchForce)
        {
            Vector2 launchPosition = _pr.Position + new Vector2(0, 0.4f);

            GameObject launchPrefab = _ammoObject == null ? _throwPrefab : _ammoObject.AmmoPrefab;
            GameObject launchObject = Instantiate(launchPrefab, launchPosition, Quaternion.identity);
            Rigidbody2D rb = launchObject.GetComponent<Rigidbody2D>();
            AudioManager.Instance.PlayClip(_throwSound, false, true);

            Vector2 direction = ((Vector3)_pr.MousePosition - rb.transform.position).normalized;
            Vector2 launchForce = (direction * (baseLaunchForce + _launchForce));

            if (launchObject.TryGetComponent(out Ammo arrow))
            {
                arrow.Setup(this, _ammoObject, launchForcePercentage, launchForce);
            }
        }

        public override string GetDescription()
        {
            return $"• Can be launched<br>{Description}";
        }
    }
}

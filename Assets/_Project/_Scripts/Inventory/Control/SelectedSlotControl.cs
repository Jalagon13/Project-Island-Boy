using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class SelectedSlotControl : MonoBehaviour
    {
        [field:SerializeField] public PlayerReference PR { get; private set; }
        [field:SerializeField] public HealthBar HealthBar { get; private set; }
        [field:SerializeField] public EnergyBar EnergyBar { get; private set; }
        [field:SerializeField] public SingleTileAction SingleTileAction { get; private set; }
        [field:SerializeField] public Collider2D PlayerEntityCollider { get; private set; }

        private PlayerInput _input;

        private void Awake()
        {
            _input = new();
            _input.Player.SecondaryAction.started += SelectedSlotAction;
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void SelectedSlotAction(InputAction.CallbackContext context)
        {
            if (PR.SelectedSlot.ItemObject != null)
                PR.SelectedSlot.ItemObject.ExecuteAction(this);
        }

        public void RestoreStat(ConsumeType cType, int value)
        {
            switch (cType)
            {
                case ConsumeType.Energy:
                    EnergyBar.AddTo(value);
                    break;
                case ConsumeType.Health:
                    HealthBar.AddTo(value);
                    break;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class SelectedSlotControl : MonoBehaviour
    {
        [field:SerializeField] public PlayerReference PR { get; private set; }
        [field:SerializeField] public HealthBar HealthBar { get; private set; }
        [field:SerializeField] public EnergyBar EnergyBar { get; private set; }
        [field:SerializeField] public SingleTileAction SingleTileAction { get; private set; }
        [field:SerializeField] public Collider2D PlayerEntityCollider { get; private set; }
        [field:SerializeField] public Tilemap WallTilemap { get; private set; }
        [field:SerializeField] public Tilemap FloorTilemap { get; private set; }
        [field:SerializeField] public Tilemap IslandTilemap { get; private set; }

        private PlayerInput _input;
        private bool _isHeldDown;
        private float _counter;
        private float _baseCoolDown = 0.17f;

        private void Awake()
        {
            _input = new();
            _input.Player.SecondaryAction.started += SelectedSlotAction;
            _input.Player.SecondaryAction.performed += IsHeldDown;
            _input.Player.SecondaryAction.canceled += IsHeldDown;
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Update()
        {
            _counter += Time.deltaTime;

            if (_counter > _baseCoolDown)
                _counter = _baseCoolDown;

            if (_isHeldDown)
                TryExecuteSlotAction();
        }

        private void IsHeldDown(InputAction.CallbackContext context)
        {
            Debug.Log("Held down");
            _isHeldDown = context.performed;
        }

        private void SelectedSlotAction(InputAction.CallbackContext context)
        {
            TryExecuteSlotAction();
        }

        private void TryExecuteSlotAction()
        {
            if (PR.SelectedSlot.ItemObject != null && _counter >= _baseCoolDown)
            {
                Debug.Log("Slot Action");
                PR.SelectedSlot.ItemObject.ExecuteAction(this);
                _counter = 0;
            }
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

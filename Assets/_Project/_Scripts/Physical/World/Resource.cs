using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    [SelectionBase]
    public class Resource : MonoBehaviour, IBreakable
    {
        [SerializeField] private string _resourceName;
        [SerializeField] private float _maxHitPoints;
        [SerializeField] private bool _breakWithAnyTool = false;
        [SerializeField] private ToolType _harvestType;
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _breakSound;
        [SerializeField] private LootTable _lootTable;

        private readonly static int _rscIdleHash = Animator.StringToHash("[Anm] RscIdle");
        private readonly static int _rscHitHash = Animator.StringToHash("[Anm] RscHit");
        private SpriteRenderer _sr;
        private Vector2 _dropPosition;
        private float _currentHitPoints;

        public ToolType BreakType { get { return _harvestType; } set { _harvestType = value; } }
        public bool BreakWithAnyTool { get { return _breakWithAnyTool; } set { _breakWithAnyTool = value; } }
        public float MaxHitPoints { get { return _maxHitPoints; } set { _maxHitPoints = value; } }
        public float CurrentHitPoints { get { return _currentHitPoints; } set { _currentHitPoints = value; } }
        public string ResourceName { get { return _resourceName; } }

        private void Awake()
        {
            _dropPosition = transform.position + new Vector3(0.5f, 0.5f, 0);
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _currentHitPoints = _maxHitPoints;
        }

        public void RscIdle()
        {
            AnimStateManager.ChangeAnimationState(GetComponent<Animator>(), _rscIdleHash);
        }

        public void RscHit()
        {
            AnimStateManager.ChangeAnimationState(GetComponent<Animator>(), _rscHitHash);
        }

        public bool Hit(float amount, ToolType toolType = ToolType.None)
        {
            if (!_breakWithAnyTool)
                if (toolType == ToolType.None || toolType != _harvestType) return false;

            AudioManager.Instance.PlayClip(_hitSound, false, true, 0.7f);
            PopupMessage.Create(transform.position, amount.ToString(), Color.yellow, new(0, 0.5f));

            RscHit();

            _currentHitPoints -= amount;

            if (_currentHitPoints <= 0)
                Break();

            return true;
        }

        public void Break()
        {
            AudioManager.Instance.PlayClip(_breakSound, false, true, 0.75f);
            _lootTable.SpawnLoot(_dropPosition);
            StopAllCoroutines();

            Destroy(gameObject);
        }
    }
}

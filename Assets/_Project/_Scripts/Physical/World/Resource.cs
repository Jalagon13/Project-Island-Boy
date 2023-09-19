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
        [SerializeField] private ToolType _harvestType;
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _breakSound;
        [SerializeField] private LootTable _lootTable;

        private SpriteRenderer _sr;
        private Vector2 _dropPosition;
        private float _currentHitPoints;

        public ToolType BreakType { get { return _harvestType; } set { _harvestType = value; } }
        public float MaxHitPoints { get { return _maxHitPoints; } set { _maxHitPoints = value; } }
        public float CurrentHitPoints { get { return _currentHitPoints; } set { _currentHitPoints = value; } }
        public string ResourceName { get { return _resourceName; } }

        private void Awake()
        {
            _dropPosition = transform.position + new Vector3(0.5f, 0.5f, 0);
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _currentHitPoints = _maxHitPoints;
        }

        public bool Hit(float amount, ToolType toolType = ToolType.None)
        {
            if(_harvestType != ToolType.Indifferent)
            {
                if (toolType == ToolType.None)
                    goto noneToolType;
                else if (toolType != _harvestType) 
                    return false;
            }

            noneToolType:

            AudioManager.Instance.PlayClip(_hitSound, false, true, 0.7f);
            PopupMessage.Create(transform.position, amount.ToString(), Color.yellow, 0.5f);

            StartCoroutine(Tremble());

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

        private IEnumerator Tremble()
        {
            for (int i = 0; i < 3; i++)
            {
                _sr.transform.localPosition += new Vector3(0.05f, 0, 0);
                yield return new WaitForSeconds(0.01f);
                _sr.transform.localPosition -= new Vector3(0.05f, 0, 0);
                yield return new WaitForSeconds(0.01f);
                _sr.transform.localPosition -= new Vector3(0.05f, 0, 0);
                yield return new WaitForSeconds(0.01f);
                _sr.transform.localPosition += new Vector3(0.05f, 0, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}

using System.Collections;
using UnityEngine;

namespace IslandBoy
{
    public class Resource : MonoBehaviour, IBreakable
    {
        [SerializeField] private float _hitPoints;
        [SerializeField] private ToolType _harvestType;
        [SerializeField] private ItemObject _dropItem;
        [SerializeField] private int _count;
        [Header("Game Feel")]
        [SerializeField] private AudioClip _chopSound;
        [SerializeField] private AudioClip _breakSound;

        private SpriteRenderer _sr;
        private Vector2 _dropPosition;

        private void Awake()
        {
            _dropPosition = transform.position + new Vector3(0, 0.5f, 0);
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        public float HitPoints { get { return _hitPoints; } set { _hitPoints = value; } }

        public bool Hit(float amount, ToolType toolType)
        {
            if (toolType != _harvestType) return false;

            AudioManager.Instance.PlayClip(_chopSound, false, true);
            StartCoroutine(Tremble());

            _hitPoints -= amount;
            if (_hitPoints <= 0)
                Break();

            return true;
        }

        public void Break()
        {
            AudioManager.Instance.PlayClip(_breakSound, false, true);
            WorldItemManager.Instance.SpawnItem(_dropPosition, _dropItem, _count);
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

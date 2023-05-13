using System.Collections;
using UnityEngine;

namespace IslandBoy
{
    public class Resource : MonoBehaviour, IBreakable
    {
        [SerializeField] private int _hitPoints;
        [SerializeField] private AudioClip _chopSound;
        [SerializeField] private AudioClip _breakSound;
        [SerializeField] private ItemObject _dropItem;
        [SerializeField] private int _count;

        private SpriteRenderer _sr;
        private Vector2 _dropPosition;
        private int _durability = 6;

        private void Awake()
        {
            _dropPosition = transform.position + new Vector3(0, 0.5f, 0);
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        public int HitPoints { get { return _hitPoints; } set { _hitPoints = value; } }

        public void Hit()
        {
            AudioManager.Instance.PlayClip(_chopSound, false, true);
            StartCoroutine(Tremble());
            _durability--;

            if(_durability <= 0)
                Break();
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

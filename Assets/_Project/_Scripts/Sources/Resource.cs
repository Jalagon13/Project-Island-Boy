using UnityEngine;

namespace IslandBoy
{
    public class Resource : MonoBehaviour, IBreakable
    {
        [SerializeField] private int _hitPoints;
        [SerializeField] private AudioClip _chopSound;
        [SerializeField] private ItemObject _dropItem;
        [SerializeField] private int _count;

        private Vector2 _dropPosition;
        private int _durability = 6;

        private void Awake()
        {
            _dropPosition = transform.position + new Vector3(0, 0.5f, 0);
        }

        public int HitPoints { get { return _hitPoints; } set { _hitPoints = value; } }

        public void Hit()
        {
            AudioManager.Instance.PlayClip(_chopSound, false, true);
            _durability--;

            if(_durability <= 0)
                Break();
        }

        public void Break()
        {
            WorldItemManager.Instance.SpawnItem(_dropPosition, _dropItem, _count);
            Destroy(gameObject);
        }
    }
}

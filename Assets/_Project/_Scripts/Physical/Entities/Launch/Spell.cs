using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Spell : MonoBehaviour
    {
        [SerializeField] private AudioClip _landSound;
        [SerializeField] private ItemParameter _powerParameter;

        private Entity _targetEntity;
        private Vector2 _targetPosition;
        private int _damage;

        private void Update()
        {
            _targetPosition = _targetEntity != null ? _targetEntity.transform.position : _targetPosition;
            transform.position = _targetPosition;
        }

        public void Setup(Entity target, SpellObject spellObject)
        {
            _targetEntity = target;
            _damage = ExtractPower(spellObject);
        }

        public void SpellHit()
        {
            AudioManager.Instance.PlayClip(_landSound, false, true, 1f, 1f);

            if(_targetEntity != null)
                _targetEntity.OnHit(ToolType.Sword, _damage);

            var entities = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach (Collider2D entity in entities)
            {
                if (entity.TryGetComponent(out Entity e))
                {
                    if (e == null) continue;
                    if(e == _targetEntity) continue;
                    e.OnHit(ToolType.Sword, (int)(_damage * 0.5f));
                }
            }

            _targetEntity = null;
        }

        public void SpellFadeAway()
        {
            Destroy(gameObject);
        }

        private int ExtractPower(ItemObject item)
        {
            var itemParams = item.DefaultParameterList;

            if (itemParams.Contains(_powerParameter))
            {
                int index = itemParams.IndexOf(_powerParameter);
                return (int)itemParams[index].Value;
            }
            Debug.LogError($"{item.Name} does not have power param so can not extract power int");
            return 0;
        }
    }
}

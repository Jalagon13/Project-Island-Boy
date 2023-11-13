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
        private int _damage;

        private void Update()
        {
            if (_targetEntity != null)
                transform.position = _targetEntity.transform.position;
        }

        public void Setup(Entity target, SpellObject spellObject)
        {
            _targetEntity = target;
            _damage = ExtractPower(spellObject);
        }

        public void BoulderLand()
        {
            AudioManager.Instance.PlayClip(_landSound, false, true, 1f, 1f);

            _targetEntity.OnClick(ToolType.Sword, _damage);
            var entities = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach (Collider2D entity in entities)
            {
                if (entity.TryGetComponent(out Entity e))
                {
                    if(e == _targetEntity) continue;
                    e.OnClick(ToolType.Sword, (int)(_damage * 0.5f));
                }
            }
        }

        public void BoulderFadeAway()
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

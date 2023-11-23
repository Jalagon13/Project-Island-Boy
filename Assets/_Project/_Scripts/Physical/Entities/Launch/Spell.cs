using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Spell : MonoBehaviour
    {
        [SerializeField] private AudioClip _landSound;
        [SerializeField] private ItemParameter _damageParameter;

        private int _damage;

        public void Setup(SpellObject spellObject)
        {
            _damage = ExtractDamage(spellObject);
        }

        public void SpellHit()
        {
            AudioManager.Instance.PlayClip(_landSound, false, true, 1f, 1f);

            var entities = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach (Collider2D entity in entities)
            {
                if (entity.TryGetComponent(out Entity e))
                {
                    if (e == null) continue;
                    e.OnHit(ToolType.Sword, _damage);
                }
            }
        }

        public void SpellFadeAway()
        {
            Destroy(gameObject);
        }

        private int ExtractDamage(ItemObject item)
        {
            var itemParams = item.DefaultParameterList;

            if (itemParams.Contains(_damageParameter))
            {
                int index = itemParams.IndexOf(_damageParameter);
                return (int)itemParams[index].Value;
            }
            Debug.LogError($"{item.Name} does not have power param so can not extract power int");
            return 0;
        }
    }
}

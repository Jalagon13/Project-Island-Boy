using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerPickupUI : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _itemPickupSignPrefab;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private float _disappearTimer;

        private List<PickupSign> _itemPickups;
        private bool _processing;

        private void Awake()
        {
            _itemPickups = new();
        }

        public void PickupUIHandle(ItemObject item, int amount)
        {
            AudioManager.Instance.PlayClip(_popSound, false, true);
            GameObject itemPickupSign = Instantiate(_itemPickupSignPrefab);
            PickupSign pickup = itemPickupSign.GetComponent<PickupSign>();

            //pickup.Initialize($"{item.Name}, {amount}");
            pickup.Initialize(item, amount, Color.white);

            _itemPickups.Add(pickup);

            if (!_processing)
                StartCoroutine(ProcessPickupUIs());
        }

        private IEnumerator ProcessPickupUIs()
        {
            _processing = true;

            while (_itemPickups.Count > 0)
            {
                yield return OverlapCheck();
            }

            _processing = false;
        }

        private IEnumerator OverlapCheck()
        {
            checkAgain:

            bool foundOverlap = false;
            PickupSign firstValPs = _itemPickups.First();
            var colliders = Physics2D.OverlapBoxAll(firstValPs.gameObject.transform.position, new Vector2(2f, 0.25f), 0.2f);

            foreach (Collider2D collider in colliders)
            {
                if (!collider.TryGetComponent(out PickupSign colliderPs)) continue;
                if (colliderPs == firstValPs) continue;

                firstValPs.gameObject.transform.position += new Vector3(0f, 0.5f, 0f);
                foundOverlap = true;

                yield return new WaitForEndOfFrame();
                
                break;
            }

            if (foundOverlap)
                goto checkAgain;

            //yield return new WaitForEndOfFrame();

            Destroy(firstValPs.gameObject, _disappearTimer);

            _itemPickups.Remove(_itemPickups.First());
        }
    }
}

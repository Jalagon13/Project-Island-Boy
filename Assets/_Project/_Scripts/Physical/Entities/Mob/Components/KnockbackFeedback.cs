using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
    public class KnockbackFeedback : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _strength = 16f, _delay = 0.15f;
        [SerializeField] private UnityEvent _onBegin, _onDone;

        public void PlayFeedback(GameObject sender)
        {
            StopAllCoroutines();
            _onBegin?.Invoke();
            Vector2 direction = (transform.position - sender.transform.position).normalized;
            _rb.AddForce(direction * _strength, ForceMode2D.Impulse);
            StartCoroutine(Reset());
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(_delay);
            _rb.velocity = Vector2.zero;
            _onDone?.Invoke();
        }
    }
}

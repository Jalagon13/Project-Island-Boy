using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Attractor : MonoBehaviour
    {
        [SerializeField] private float _attractSpeed;

        private bool _canAttract;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.65f);
            _canAttract = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Collect") && _canAttract)
            {
                transform.root.position = Vector2.MoveTowards(transform.root.position, collision.transform.root.position, _attractSpeed * Time.deltaTime);
            }
        }
    }
}

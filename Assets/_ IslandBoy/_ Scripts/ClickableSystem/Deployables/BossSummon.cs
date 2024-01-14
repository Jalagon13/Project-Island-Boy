using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class BossSummon : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnPrefab;
        [SerializeField] private float _spawnTimer;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_spawnTimer);

            Instantiate(_spawnPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Sapling : MonoBehaviour
    {
        [SerializeField] private float _growthTimerSec;
        [SerializeField] private GameObject _treePrefab;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_growthTimerSec);
            Instantiate(_treePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

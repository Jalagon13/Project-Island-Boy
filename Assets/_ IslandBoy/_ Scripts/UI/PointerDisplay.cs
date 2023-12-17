using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PointerDisplay : MonoBehaviour
    {
        private IEnumerator Animation()
        {
            transform.localScale = Vector3.one * 70f;
            yield return new WaitForSeconds(0.5f);
            transform.localScale = Vector3.one * 85f;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Animation());
        }

        private void OnEnable()
        {
            StartCoroutine(Animation());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}

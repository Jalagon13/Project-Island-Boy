using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TitleLogo : MonoBehaviour
    {
        private IEnumerator Animation()
        {
            float scale = 1f;
            float time = 0.12f;
            for (int i = 0; i < 11; i++)
            {
                transform.localScale = Vector3.one * scale;
                yield return new WaitForSeconds(time);
                scale += 0.01f;
            }

            yield return new WaitForSeconds(time);

            for (int i = 10; i >= 0; i--)
            {
                scale -= 0.01f;
                transform.localScale = Vector3.one * scale;
                yield return new WaitForSeconds(time);
            }

            yield return new WaitForSeconds(time);

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

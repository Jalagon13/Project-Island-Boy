using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class Sign : MonoBehaviour
    {
        [TextArea]
        [SerializeField] private string _text;
        [SerializeField] private TextMeshProUGUI _signText;

        private GameObject _tutorialObject;

        private void Awake()
        {
            _signText.text = _text;
            _tutorialObject = transform.GetChild(3).gameObject;
        }

        private IEnumerator Start()
        {
            _tutorialObject.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.5f);
            _tutorialObject.transform.localScale = new(1.15f, 1.15f, 1.15f);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Start());
        }

        public void StopTutorialIndicator()
        {
            StopAllCoroutines();
            Destroy(_tutorialObject);
        }
    }
}

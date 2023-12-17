using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class Sign : MonoBehaviour
    {
        [TextArea]
        [SerializeField] private string[] _text;
        [SerializeField] private TextMeshProUGUI _signText;
        [SerializeField] private bool _disableTextOverride;
        
        private GameObject _tutorialObject;
        private int _counter = 0;

        private void Awake()
        {
            _tutorialObject = transform.GetChild(3).gameObject;

            if (_disableTextOverride) return;

            _signText.text = _text[0];
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

        public void NextText()
        {
            _counter++;
            if (_counter >= _text.Length)
                _counter = 0;

            _signText.text = _text[_counter];
        }

        public void PreviousText()
        {
            _counter--;
            if (_counter < 0)
                _counter = _text.Length - 1;

            _signText.text = _text[_counter];
        }
    }
}
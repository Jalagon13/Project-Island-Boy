using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class Sign : MonoBehaviour
    {
        [TextArea]
        [SerializeField] private string[] _text;
        [SerializeField] private TextMeshProUGUI _signText;
        [SerializeField] private GameObject _tutorialObject;
        [SerializeField] private Button _nextButton, _backButton;
        
        private int _counter = 0;

        private void Awake()
        {
            if (_signText != null)
            {
            	_signText.text = _text[0];
            }
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
            if (_counter >= _text.Length - 1)
                _nextButton.interactable = false;
            else if (!_backButton.interactable)
                _backButton.interactable = true;

            _signText.text = _text[_counter];
        }

        public void PreviousText()
        {
            _counter--;
            if (_counter <= 0)  
                _backButton.interactable = false;
            else if(!_nextButton.interactable)
                _nextButton.interactable = true;

            _signText.text = _text[_counter];
        }

        // called when text box is exited so dialogue can be cycled through again
        public void ResetText()
        {
            _counter = 0;
            _signText.text = _text[0];
            _nextButton.interactable = true;
            _backButton.interactable = false;
        }
    }

}

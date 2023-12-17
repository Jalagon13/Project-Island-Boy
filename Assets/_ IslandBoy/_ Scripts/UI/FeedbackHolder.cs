using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class FeedbackHolder : MonoBehaviour
    {
        [SerializeField] private float _feedbackDurationSec;
        [SerializeField] private List<HousingCheckButton> _checkButtons;

        private TextMeshProUGUI _feedbackText;
        private Image _bgImage;

        private void Awake()
        {
            _bgImage = transform.GetChild(0).GetComponent<Image>();
            _feedbackText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            ActivateUI(false);
        }

        public void DisplayFeedback(string text, Color color)
        {
            PopulateText(text, color);
            ActivateUI(true);
            ActivateButtons(false);
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(_feedbackDurationSec);

            ActivateUI(false);
            ActivateButtons(true);
        }

        private void ActivateButtons(bool foo)
        {
            foreach (HousingCheckButton button in _checkButtons)
            {
                if (foo)
                    button.EnableButton();
                else
                    button.DisableButton();
            }
        }

        private void PopulateText(string text, Color textColor)
        {
            _feedbackText.text = text;
            _feedbackText.color = textColor;
        }

        private void ActivateUI(bool foo)
        {
            _feedbackText.enabled = foo;
            _bgImage.enabled = foo;
        }
    }
}

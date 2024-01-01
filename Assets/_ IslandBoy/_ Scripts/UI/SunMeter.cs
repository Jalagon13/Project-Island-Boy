using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IslandBoy
{
	public class SunMeter : MonoBehaviour
	{
		[SerializeField] private RectTransform _marker;
		[SerializeField] private RectTransform _panel;
		[SerializeField] private Vector2 _markerStartPosition;
		[SerializeField] private Vector2 _markerEndPosition;

		private Queue<string> _endDaySlides = new();

		private void Awake()
		{
			GameSignals.DAY_END.AddListener(EndDay);
			GameSignals.RESIDENT_UPDATE.AddListener(ResidentUpdate);
		}

		private void OnDestroy()
		{
			GameSignals.DAY_END.RemoveListener(EndDay);
			GameSignals.RESIDENT_UPDATE.RemoveListener(ResidentUpdate);
		}

		private void Start()
		{
			ResetDay();
			PanelEnabled(false);
		}

		private void Update()
		{
			MoveMarker();
		}
		
		private void ResidentUpdate(ISignalParameters parameters)
		{
			if(parameters.HasParameter("Message"))
			{
				string message = (string)parameters.GetParameter("Message");
				AddEndDaySlide(message);
			}
		}

		private void MoveMarker()
		{
			float xValue = Mathf.Lerp(_markerStartPosition.x, _markerEndPosition.x, TimeManager.Instance.CurrentDayRatio);
			_marker.anchoredPosition = new Vector2(xValue, _markerStartPosition.y);
		}

		public void StartDay() // connected to continue button
		{
			ResetDay();
			PanelEnabled(false);
			GameSignals.DAY_START.Dispatch();
		}

		private void ResetDay()
		{
			_marker.localPosition = _markerStartPosition;
		}

		public void AddEndDaySlide(string text)
		{
			_endDaySlides.Enqueue(text);
		}

		public void ClearEndDaySlides()
		{
			_endDaySlides.Clear();
		}

		[Button("End Day")]
		public void EndDay(ISignalParameters parameters)
		{
			PanelEnabled(true);
			StartCoroutine(TextSequence());
		}

		private IEnumerator TextSequence()
		{
			var text = _panel.GetChild(0).GetComponent<TextMeshProUGUI>();
			var button = _panel.GetChild(1).GetComponent<Button>();

			text.gameObject.SetActive(false);
			button.gameObject.SetActive(false);

			yield return new WaitForSeconds(1f);

			text.text = "Day has ended.";
			text.gameObject.SetActive(true);

			yield return new WaitForSeconds(2f);

			foreach (string slide in _endDaySlides)
			{
				text.text = slide;
				yield return new WaitForSeconds(2f);
			}

			text.text = "Health and Energy replenished!";
			button.gameObject.SetActive(true);

			ClearEndDaySlides();
		}

		private void PanelEnabled(bool _)
		{
			_panel.gameObject.SetActive(_);
		}
	}
}

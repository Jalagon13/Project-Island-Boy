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
		[SerializeField] private PlayerObject _pr;
		[SerializeField] private float _dayDurationInSec;
		[SerializeField] private float _percentTillSunset = 0.7f;
		[Range(0, 1f)]
		[SerializeField] private float _percentTillSleepWarning;
		[Range(0, 1f)]
		[SerializeField] private float _debugDayPercentage;
		[Header("Editor Stuff")]
		[SerializeField] private RectTransform _marker;
		[SerializeField] private RectTransform _panel;
		[SerializeField] private Sprite _sunSprite;
		[SerializeField] private Vector2 _markerStartPosition;
		[SerializeField] private Vector2 _markerEndPosition;

		private Timer _timer;
		private Volume _globalVolume;
		private float _duration;
		private float _phasePercent;
		private bool _isDay, _hasDisplayedWarning;
		private Queue<string> _endDaySlides = new();

		private void Awake()
		{
			_globalVolume = FindFirstObjectByType<Volume>();
			_timer = new(_dayDurationInSec);
			_timer.OnTimerEnd += OutOfTime;

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
			UpdateMarker(_sunSprite);
		}

		private void Update()
		{
			if(_timer.IsPaused) return;
			
			_timer.Tick(Time.deltaTime);
			_phasePercent = (_duration - _timer.RemainingSeconds) / _duration;

			MoveMarker();
			VolumeHandle();
			if (_phasePercent > _percentTillSleepWarning && !_hasDisplayedWarning)
			{
				PopupMessage.Create(_pr.Position, "I need to sleep soon..", Color.cyan, new(0f, 0.75f), 1.5f);
				_hasDisplayedWarning = true;
			}
		}
		
		private void ResidentUpdate(ISignalParameters parameters)
		{
			if(parameters.HasParameter("Message"))
			{
				string message = (string)parameters.GetParameter("Message");
				AddEndDaySlide(message);
			}
		}

		private void OutOfTime()
		{
			GameSignals.DAY_END.Dispatch();
			//GameSignals.DAY_OUT_OF_TIME.Dispatch();
		}

		private void VolumeHandle()
		{
			if(SceneManager.GetActiveScene().buildIndex == 2)
			{
				if (_phasePercent <= 0.4f)
				{
					float percent = _phasePercent / 0.1f;
					_globalVolume.weight = Mathf.Lerp(0.5f, _isDay ? 0 : 1, percent);
				}
				else if(_phasePercent >= _percentTillSunset && _phasePercent <= 1f)
				{
					float per = (1f - _phasePercent);
					float p = (1 - _percentTillSunset) - per;
					float x = p / (1 - _percentTillSunset);

					_globalVolume.weight = Mathf.Lerp(_isDay ? 0 : 1, 0.5f, x);
				}
			}
			else if(SceneManager.GetActiveScene().buildIndex == 3)
			{
				_globalVolume.weight = 1;
			}
		}

		private void MoveMarker()
		{
			float xValue = Mathf.Lerp(_markerStartPosition.x, _markerEndPosition.x, _phasePercent);
			_marker.anchoredPosition = new Vector2(xValue, _markerStartPosition.y);
		}

		public void StartDay() // connected to continue button
		{
			ResetDay();
			PanelEnabled(false);
			UpdateMarker(_sunSprite);

			GameSignals.DAY_START.Dispatch();
		}

		private void ResetDay()
		{
			_marker.localPosition = _markerStartPosition;
			_timer.RemainingSeconds = _dayDurationInSec;
			_duration = _dayDurationInSec;
			_hasDisplayedWarning = false;
			_timer.IsPaused = false;
			_timer.Tick(_dayDurationInSec * _debugDayPercentage);
			_isDay = true;
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
		public void EndDay(ISignalParameters parameters) // connected to bed
		{
			_timer.IsPaused = true;

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

			text.text = "Day has ended!";
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

		private void UpdateMarker(Sprite sprite)
		{
			_marker.GetComponent<Image>().sprite = sprite;
		}
	}
}

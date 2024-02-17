using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
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
		[SerializeField] private PlayerObject _po;
		[SerializeField] private AudioClip _xpSound;
		[SerializeField] private RectTransform _marker;
		[SerializeField] private RectTransform _panel;
		[SerializeField] private Vector2 _markerStartPosition;
		[SerializeField] private Vector2 _markerEndPosition;

		private Queue<string> _endDaySlides = new();
		private Dictionary<string, EntityData> _entityTally = new();
		private int _grandTotalXpGain;

		public class EntityData
		{
			public int XpAmount;
			public int SlainCount;
			
			public EntityData(int xpAmount)
			{
				XpAmount = xpAmount;
				SlainCount = 1;
			}
		}
		
		private void Awake()
		{
			GameSignals.DAY_END.AddListener(EndDay);
			GameSignals.RESIDENT_UPDATE.AddListener(ResidentUpdate);
			GameSignals.ENTITY_DIED.AddListener(RegisterEntityDeath);
		}

		private void OnDestroy()
		{
			GameSignals.DAY_END.RemoveListener(EndDay);
			GameSignals.RESIDENT_UPDATE.RemoveListener(ResidentUpdate);
			GameSignals.ENTITY_DIED.RemoveListener(RegisterEntityDeath);
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
		
		private void RegisterEntityDeath(ISignalParameters parameters)
		{
			Entity entity = (Entity)parameters.GetParameter("Entity");
			
			if(!_entityTally.ContainsKey(entity.EntityName))
			{
				EntityData ed = new(entity.XpAmount);
				_entityTally.Add(entity.EntityName, ed);
			}
			else
				_entityTally[entity.EntityName].SlainCount++;
		}
		
		private void ResidentUpdate(ISignalParameters parameters)
		{
			if(parameters.HasParameter("Message"))
			{
				string message = (string)parameters.GetParameter("Message");
				AddEndDaySlide(message);
				Debug.Log("Mvoed in");
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
			PlayerGoldController.Instance.AddCurrency(_grandTotalXpGain, _po.Position + new Vector2(0.5f, 0.5f));
			_grandTotalXpGain = 0;
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
			_endDaySlides = new();
		}

		[Button("End Day")]
		public void EndDay(ISignalParameters parameters)
		{
			PanelEnabled(true);
			StartCoroutine(TextSequence());
		}

		private IEnumerator TextSequence()
		{
			// switch (Player.RESTED_STATUS)
			// {
			// 	case RestedStatus.Good:
			// 		AddEndDaySlide("You got a good night's rest!");
			// 		break;
			// 	case RestedStatus.Okay:
			// 		AddEndDaySlide("Your sleep last night was not the best..");
			// 		break;
			// 	case RestedStatus.Bad:
			// 		AddEndDaySlide("You passed out at the end of the day...");
			// 		break;
			// }
			
			var text = _panel.GetChild(0).GetComponent<TextMeshProUGUI>();
			var button = _panel.GetChild(1).GetComponent<Button>();

			text.gameObject.SetActive(false);
			button.gameObject.SetActive(false);

			yield return new WaitForSeconds(1f);

			text.text = "Day has ended.";
			text.gameObject.SetActive(true);

			// yield return new WaitForSeconds(2f);
			Debug.Log(_endDaySlides.Count);
			
			foreach (string slide in _endDaySlides)
			{
				text.text = slide;
				yield return new WaitForSeconds(2f);
			}
			
			ClearEndDaySlides();
			
			if(_entityTally.Count > 0)
			{
				string monsterWording = $"<color=red>Monsters Slain:</color=red><br><br>";
				int grandTotalXp = 0;
				text.text = monsterWording;
				
				yield return new WaitForSeconds(2.5f);
				
				foreach(var item in _entityTally)
				{
					monsterWording += $"{item.Key} ({item.Value.SlainCount}): XP: <color=green>{item.Value.SlainCount * item.Value.XpAmount}</color=green><br>";
					grandTotalXp += item.Value.SlainCount * item.Value.XpAmount;
					text.text = monsterWording;
					MMSoundManagerSoundPlayEvent.Trigger(_xpSound, MMSoundManager.MMSoundManagerTracks.UI, default);
					yield return new WaitForSeconds(0.5f);
				}
				
				float multiplier = 0;
				
				if(Player.RESTED_STATUS != RestedStatus.Good)
				{
					if(Player.RESTED_STATUS == RestedStatus.Bad)
					{
						multiplier = 0.25f;
					}
					else if(Player.RESTED_STATUS == RestedStatus.Okay)
					{
						multiplier = 0.5f;
					}
				}
				
				if(multiplier != 0)
				{
					// MMSoundManagerSoundPlayEvent.Trigger(_xpSound, MMSoundManager.MMSoundManagerTracks.UI, default);
					monsterWording += $"<br>Sleep Penalty Xp Multiplier: <color=red>x{multiplier}</color=red>";
					// text.text = monsterWording;
					// yield return new WaitForSeconds(1f);
				}
				
				MMSoundManagerSoundPlayEvent.Trigger(_xpSound, MMSoundManager.MMSoundManagerTracks.UI, default);
				if(multiplier != 0)
					monsterWording += $"<br>Total XP Gained: <color=green>{grandTotalXp}</color=green> x <color=red>{multiplier}</color=red> = <color=green>{(int)(grandTotalXp * multiplier)}</color=green>";
				else
					monsterWording += $"<br>Total XP Gained: <color=green>{grandTotalXp}</color=green>";
					
				grandTotalXp = (int)(grandTotalXp * multiplier);
				_grandTotalXpGain = grandTotalXp;
				text.text = monsterWording;
				
				yield return new WaitForSeconds(3f);
			}
			
			button.gameObject.SetActive(true);

			ClearEndDaySlides();
			
			_entityTally.Clear();
			_entityTally = new();
		}

		private void PanelEnabled(bool _)
		{
			_panel.gameObject.SetActive(_);
		}
	}
}

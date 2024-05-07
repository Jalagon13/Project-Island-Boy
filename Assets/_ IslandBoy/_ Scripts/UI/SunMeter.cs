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
		private int _energyPenalityCount;

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
			_po.DayNum = 0;
			GameSignals.DAY_END.AddListener(EndDay);
			GameSignals.RESIDENT_UPDATE.AddListener(ResidentUpdate);
			GameSignals.ENTITY_DIED.AddListener(RegisterEntityDeath);
			GameSignals.RESOURCE_CLEARED.AddListener(RegisterResource);
			GameSignals.PLAYER_DIED.AddListener(ApplyEnergyPenality);
		}

		private void OnDestroy()
		{
			GameSignals.DAY_END.RemoveListener(EndDay);
			GameSignals.RESIDENT_UPDATE.RemoveListener(ResidentUpdate);
			GameSignals.ENTITY_DIED.RemoveListener(RegisterEntityDeath);
			GameSignals.RESOURCE_CLEARED.RemoveListener(RegisterResource);
			GameSignals.PLAYER_DIED.RemoveListener(ApplyEnergyPenality);
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
		
		private void ApplyEnergyPenality(ISignalParameters parameters)
		{
			_energyPenalityCount++;
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
		
		public void RegisterResource(ISignalParameters parameters)
		{
			Resource rsc = (Resource)parameters.GetParameter("Resource");
			
			if(!_entityTally.ContainsKey(rsc.RscName))
			{
				EntityData ed = new(rsc.MaxHitPoints);
				_entityTally.Add(rsc.RscName, ed);
			}
			else
				_entityTally[rsc.RscName].SlainCount++;
		}
		
		private void ResidentUpdate(ISignalParameters parameters)
		{
			if(parameters.HasParameter("Message"))
			{
				string message = (string)parameters.GetParameter("Message");
				AddEndDaySlide(message);
				// Debug.Log("Mvoed in");
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
			
			StartCoroutine(Delay());
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForSeconds(2f);
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
			_po.DayNum += 1;
			PanelEnabled(true);
			StartCoroutine(TextSequence());
		}

		private IEnumerator TextSequence()
		{
			string endOfDayTxt = "<color=#6ed0f7>End of Day " + _po.DayNum + "<br><br></color=#6ed0f7>";

			var text = _panel.GetChild(0).GetComponent<TextMeshProUGUI>();
			var button = _panel.GetChild(1).GetComponent<Button>();

			text.gameObject.SetActive(false);
			button.gameObject.SetActive(false);

			yield return new WaitForSeconds(1f);

			text.text = endOfDayTxt;
			MMSoundManagerSoundPlayEvent.Trigger(_xpSound, MMSoundManager.MMSoundManagerTracks.UI, default);
			text.gameObject.SetActive(true);

			yield return new WaitForSeconds(1.5f);

			// switch (Player.RESTED_STATUS)
			// {
			// 	case RestedStatus.Good:
			// 		endOfDayTxt += "You got a good night's rest!";
			// 		break;
			// 	case RestedStatus.Okay:
			// 		endOfDayTxt += "You didn't sleep very well last night...";
			// 		break;
			// 	case RestedStatus.Bad:
			// 		endOfDayTxt += "You passed out at the end of the day...";
			// 		break;
			// }

			// text.text = endOfDayTxt;
			// yield return new WaitForSeconds(2f);

			//AddEndDaySlide(endOfDayTxt);
			// yield return new WaitForSeconds(2f);
			// Debug.Log(_endDaySlides.Count);

			/*foreach (string slide in _endDaySlides)
			{
				text.text = slide;
				yield return new WaitForSeconds(2f);
			}
			ClearEndDaySlides();*/

			if (_entityTally.Count > 0)
			{
				yield return new WaitForSeconds(1f);
				string monsterWording = $"<color=orange>Resources Collected:</color=orange><br><br>";
				int grandTotalXp = 0;
				text.text = monsterWording;
				
				yield return new WaitForSeconds(1.5f);
				
				foreach(var item in _entityTally)
				{
					monsterWording += $"{item.Key}: <color=orange>{item.Value.SlainCount}</color=orange><br>";
					grandTotalXp += item.Value.SlainCount;
					text.text = monsterWording;
					MMSoundManagerSoundPlayEvent.Trigger(_xpSound, MMSoundManager.MMSoundManagerTracks.UI, default);
					yield return new WaitForSeconds(0.5f);
				}
				
				monsterWording += $"<br><color=orange>Resource Total: {grandTotalXp}</color=orange><br>";
				MMSoundManagerSoundPlayEvent.Trigger(_xpSound, MMSoundManager.MMSoundManagerTracks.UI, default);
				text.text = monsterWording;
				
				yield return new WaitForSeconds(0.5f);
				
				monsterWording += $"<color=green>XP Reward: {grandTotalXp}</color=green>";
				
				MMSoundManagerSoundPlayEvent.Trigger(_xpSound, MMSoundManager.MMSoundManagerTracks.UI, default);
				
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

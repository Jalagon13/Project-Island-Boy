using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace IslandBoy
{
	public class DynamicLight : MonoBehaviour
	{
		[InfoBox("When sum of DayLight rgb is below this threshhold, turn on light. (each rgb value is represented from 0 to 1)")]
		[Range(0, 3)]
		[SerializeField] private float _rgbThreshhold;
		
		private Light2D _light;
		
		private void Awake()
		{
			_light = GetComponent<Light2D>();
		}
		
		private void OnEnable()
		{
			_light.enabled = false;
		}
		
		private void Update()
		{
			if(TimeManager.Instance.DayCycleHandler != null)
			{
				Color dayColor = TimeManager.Instance.DayCycleHandler.DayLight.color;
				
				float r = dayColor.r;
				float g = dayColor.g;
				float b = dayColor.b;
				float rgbSum = r + b + g;
				
				_light.enabled = rgbSum < _rgbThreshhold;
			}
		}
	}
}

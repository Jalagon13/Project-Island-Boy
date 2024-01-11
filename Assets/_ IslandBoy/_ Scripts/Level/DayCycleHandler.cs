using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace IslandBoy
{
	public class DayCycleHandler : MonoBehaviour
	{
		[Header("Day Light")]
		public Light2D DayLight;
		public Gradient DayLightGradient;
		public Light2D AmbientLight;
		public Gradient AmbientLightGradient;
		
		private void OnEnable()
		{
			StartCoroutine("Delay");
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			TimeManager.Instance.DayCycleHandler = this;
		}
		
		public void Tick()
		{
			UpdateLight(TimeManager.Instance.CurrentDayRatio);
		}
		
		private void UpdateLight(float ratio)
		{
			if(DayLight != null)
				DayLight.color = DayLightGradient.Evaluate(ratio);
				
			if(AmbientLight != null)
				AmbientLight.color = AmbientLightGradient.Evaluate(ratio);
		}
	}
}

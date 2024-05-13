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
		[SerializeField] private PlayerObject _po;
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
			_po.TimeController.DayCycleHandler = this;
		}
		
		public void Tick()
		{
			UpdateLight(_po.TimeController.CurrentDayRatio);
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

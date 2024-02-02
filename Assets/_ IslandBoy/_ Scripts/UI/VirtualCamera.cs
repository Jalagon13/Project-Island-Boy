using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace IslandBoy
{
	public class VirtualCamera : MonoBehaviour
	{
		private CinemachineConfiner _confiner;
		
		private void Awake() 
		{
			_confiner = GetComponent<CinemachineConfiner>();
			
			GameSignals.CONFINER_UPDATED.AddListener(UpdateConfiner);
		}
		
		private void OnDestroy() 
		{
			GameSignals.CONFINER_UPDATED.RemoveListener(UpdateConfiner);
		}
		
		private void UpdateConfiner(ISignalParameters parameters)
		{
			if(parameters.HasParameter("Confiner"))
			{
				_confiner.m_BoundingShape2D = (PolygonCollider2D)parameters.GetParameter("Confiner");
			}
		}
	}
}

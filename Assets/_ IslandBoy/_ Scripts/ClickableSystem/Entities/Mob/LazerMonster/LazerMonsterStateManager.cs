using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class LazerMonsterStateManager : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		
		private void Start() 
		{
			
		}
		
		private void FireSequence()
		{
			// teleport to random location near player
			// charge up for 3 seconds
				// calcualte aim on player during this phase 
				// while charging show red line
			// stop aim at location then fire lazer
				// lazer lasts for 1 second
			// stay idle for 3 seconds
			// repeat
		}
	}
}

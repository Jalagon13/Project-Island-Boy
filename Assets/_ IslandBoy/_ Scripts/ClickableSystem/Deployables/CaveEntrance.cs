using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class CaveEntrance : MonoBehaviour
	{
		[SerializeField] private string _nextScene;
		
		public void EnterCave()
		{
			Signal signal = GameSignals.CHANGE_SCENE;
			signal.ClearParameters();
			signal.AddParameter("NextScene", _nextScene);
			signal.Dispatch();
		}
	}
}

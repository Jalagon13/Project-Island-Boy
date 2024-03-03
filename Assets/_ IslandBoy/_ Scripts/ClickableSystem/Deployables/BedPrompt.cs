using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class BedPrompt : MonoBehaviour
	{
		[SerializeField] private Bed _bed;
		[SerializeField] private GameObject _npcView;
		
		private void OnEnable()
		{
			if(_bed.InValidSpace())
			{
				_npcView.SetActive(true);
			}
			else
			{
				_npcView.SetActive(false);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public class CaveManager : MonoBehaviour
	{
		[SerializeField] private LevelGenerator _levelPrefab;
		
		private int _levelIndex;
		
		[Button("CreateLevel")]
		private void CreateLevel()
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(false);
			}
			
			var level = Instantiate(_levelPrefab, transform);
			SetActiveLevel(level.gameObject.transform.GetSiblingIndex());
		} 
		
		[Button("SetActiveLevel")]
		private void SetActiveLevel(int index)
		{
			foreach (Transform child in transform)
			{
				if(child.GetSiblingIndex() == index)
				{
					child.gameObject.SetActive(true);
					_levelIndex = index;
					return;
				}
			}
			
			Debug.LogError($"Could not find level of index [{index}]");
		}
		
		[Button("Reset")]
		private void Reset()
		{
			_levelIndex = 0;
		}
		
		private void ResetCaveLevels()
		{
			
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public class CaveManager : MonoBehaviour
	{
		[SerializeField] private LevelGenerator _levelPrefab;
		[SerializeField] private string _surfaceSceneName;
		
		private int _levelIndex = 0;
		
		private static CaveManager _instance;
		
		public static CaveManager Instance => _instance;
		
		private void Awake()
		{
			_instance = this;
		}
		
		private void OnEnable()
		{
			// LoadExistingLevel(LevelControl.CaveLevelToLoad);
		}
		
		[Button("Descend Floor")]
		public void DescendFloor()
		{
			int indexTested = _levelIndex + 1;
			
			if(indexTested > -1 && indexTested < transform.childCount)
			{
				_levelIndex++;
				LoadExistingLevel(_levelIndex);
			}
		}
		
		[Button("Ascend Floor")]
		public void AscendFloor()
		{
			if(_levelIndex == 0)
			{
				SwitchScene();
				return;
			}
			
			int indexTested = _levelIndex - 1;
			
			if(indexTested > -1 && indexTested < transform.childCount)
			{
				_levelIndex--;
				LoadExistingLevel(_levelIndex);
			}
		}
		
		private void SwitchScene()
		{
			Signal signal = GameSignals.CHANGE_SCENE;
			signal.ClearParameters();
			signal.AddParameter("NextScene", _surfaceSceneName);
			signal.Dispatch();
		}
		
		
		[Button("Load Existing Level")]
		private void LoadExistingLevel(int index)
		{
			if(index > -1 && index < transform.childCount)
			{
				DeActivateAllLevels();
				SetLevelActive(index);	
			}
		}
		
		private int InstantiateLevel()
		{
			return Instantiate(_levelPrefab, transform).gameObject.transform.GetSiblingIndex();
		}
		
		private void SetLevelActive(int index)
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
		
		private void DeActivateAllLevels()
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(false);
			}
		}
		
		[Button("Reset Cave Levels")]
		private void ResetCaveLevels()
		{
			_levelIndex = 0;
			
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}
	}
}

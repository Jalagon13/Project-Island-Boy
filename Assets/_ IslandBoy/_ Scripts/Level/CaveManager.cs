using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public class CaveManager : MonoBehaviour
	{
		[SerializeField] private LevelGenerator _levelPrefab;
		[SerializeField] private Transform _rewardRoom;
		
		private int _levelIndex = 0;
		private int _rewardLevelIndex = 4;
		
		[Button("Create Level")]
		private void CreateLevel()
		{
			DeActivateAllLevels();
			SetRewardRoomActive(_levelIndex == _rewardLevelIndex - 1);
			
			if(_levelIndex != _rewardLevelIndex - 1)
			{
				SetLevelActive(InstantiateLevel());
			}
		} 
		
		[Button("Load Existing Level")]
		private void LoadExistingLevel(int index)
		{
			DeActivateAllLevels();
			SetRewardRoomActive(index == _rewardLevelIndex);
			
			if(index != _rewardLevelIndex)
			{
				SetLevelActive(index);
				return;
			}
		}
		
		private int InstantiateLevel()
		{
			return Instantiate(_levelPrefab, transform).gameObject.transform.GetSiblingIndex();
		}
		
		private void SetRewardRoomActive(bool _)
		{
			_rewardRoom.gameObject.SetActive(_);
		}
		
		private void SetLevelActive(int index)
		{
			if(index == _rewardLevelIndex)
				SetRewardRoomActive(true);
			
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
			SetRewardRoomActive(false);
			
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}
	}
}

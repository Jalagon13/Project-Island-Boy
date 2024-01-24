using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public class Quest : SerializedMonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private AudioClip _slotCompleteSound;
		[SerializeField] private AudioClip _questCompleteSound;
		[SerializeField] private ItemObject _questRewardItem;
		[SerializeField] private int _rewardAmount;
		[SerializeField] private Dictionary<ItemObject, int> _questItems = new();
		
		public Dictionary<ItemObject, int> QuestItems => _questItems;
		
		public void RemoveReq(ItemObject item)
		{
			_questItems.Remove(item);
			
			if(_questItems.Count <= 0)
			{
				CompleteQuest();
				return;
			}
			
			MMSoundManagerSoundPlayEvent.Trigger(_slotCompleteSound, MMSoundManager.MMSoundManagerTracks.UI, default);
		}
		
		private void CompleteQuest()
		{
			_po.Inventory.AddItem(_questRewardItem, _rewardAmount);
			PopupMessage.Create(_po.Position, $"{_questRewardItem.Name} added to inventory!", Color.green, Vector2.up, 3f);
			MMSoundManagerSoundPlayEvent.Trigger(_questCompleteSound, MMSoundManager.MMSoundManagerTracks.UI, default);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public class GiveMoneyOnDestroy : MonoBehaviour
	{
		[MinMaxSlider(0, 99, true)]
		[SerializeField] private Vector2 _amount;
		[SerializeField] private AudioClip _goldSound;
		private void OnDestroy()
		{
			int amount = Random.Range((int)_amount.x, (int)_amount.y);
			PlayerGoldController.AddCurrency(amount);
			PopupMessage.Create(transform.position, $"+${amount}", Color.green, Vector2.up, 1f);
			MMSoundManagerSoundPlayEvent.Trigger(_goldSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
		}
	}
}

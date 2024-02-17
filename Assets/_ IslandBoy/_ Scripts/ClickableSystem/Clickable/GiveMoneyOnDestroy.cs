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

		public void GiveMoney()
		{
			// int amount = Random.Range((int)_amount.x, (int)_amount.y);
			// PlayerGoldController.Instance.AddCurrency(amount, transform.position);
		}
	}
}

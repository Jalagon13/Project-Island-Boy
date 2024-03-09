using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class ArmorListener : MonoBehaviour
	{
		[SerializeField] private ArmorType _armorType;
		private SpriteRenderer _leadSr;
		private SpriteRenderer _armorSr;
		private Dictionary<string, Sprite> _armorSprites;
		
		private void Awake()
		{
			_leadSr = transform.parent.GetComponent<SpriteRenderer>();
			_armorSr = GetComponent<SpriteRenderer>();
			
			GameSignals.ARMOR_EQUIPPED.AddListener(OnArmorEquipped);
			GameSignals.ARMOR_UNEQUIPPED.AddListener(OnArmorUnEquipped);
		}
		
		private void OnDestroy()
		{
			GameSignals.ARMOR_EQUIPPED.RemoveListener(OnArmorEquipped);
			GameSignals.ARMOR_UNEQUIPPED.RemoveListener(OnArmorUnEquipped);
		}
		
		private void LateUpdate()
		{
			_armorSr.enabled = _leadSr.enabled;
			
			if(_armorSr.enabled)
			{
				if(CurrentSpriteName(_armorSr) != CurrentSpriteName(_leadSr))
				{
					Sprite sprite;
					if(_armorSprites.TryGetValue(CurrentSpriteName(_leadSr), out sprite))
						_armorSr.sprite = sprite;
					else
						_armorSr.sprite = null;
				}
			}
			
		}
		
		private void OnArmorEquipped(ISignalParameters parameters)
		{
			
		}
		
		private void OnArmorUnEquipped(ISignalParameters parameters)
		{
			
		}
		
		private string CurrentSpriteName(SpriteRenderer sr)
		{
			return sr.sprite.name;
		}
	}
}

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
		private Dictionary<string, Sprite> _armorSprites = new();
		
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
				_armorSr.flipX = _leadSr.flipX;
				
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
			var armorObject = (ArmorObject)parameters.GetParameter("ArmorObject");
			
			if(armorObject.ArmorType == _armorType)
			{
				// equip armor
				_armorSprites.Clear();
				_armorSprites = new();
				
				var armorSprites = armorObject.SpriteList;
				
				foreach (Sprite sprite in armorSprites)
				{
					_armorSprites.Add(sprite.name, sprite);
				}
			}
		}
		
		private void OnArmorUnEquipped(ISignalParameters parameters)
		{
			var armorObject = (ArmorObject)parameters.GetParameter("ArmorObject");
			
			if(armorObject.ArmorType == _armorType)
			{
				_armorSprites.Clear();
				_armorSprites = new();
				_armorSr.sprite = null;
			}
		}
		
		private string CurrentSpriteName(SpriteRenderer sr)
		{
			if(sr.sprite == null)
				return string.Empty;
			
			return sr.sprite.name;
		}
	}
}

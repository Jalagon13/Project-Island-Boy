using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
	public class DynamicTilemap : MonoBehaviour
	{
		private Tilemap _tilemap;
		
		private Dictionary<Vector3Int, TileData> _data = new();
		
		public class TileData
		{
			public RuleTileExtended RuleTile;
			
			private int _currentHitPoints;
			
			public TileData(RuleTileExtended rte)
			{
				RuleTile = rte;
				_currentHitPoints = rte.MaxHitPoints;
			}
			
			public void Hit()
			{
				_currentHitPoints--;
			}
			
			public bool IsDestroyed()
			{
				return _currentHitPoints <= 0;
			}
		}
		
		private void Awake()
		{
			_tilemap = GetComponent<Tilemap>();
		}
		
		public void Hit(Vector3Int pos)
		{
			if(IsHittable(pos))
			{
				if(!DataContains(pos))
				{
					RuleTileExtended tile = _tilemap.GetTile<RuleTileExtended>(pos);
					TileData td = new(tile);
					_data.Add(pos, td);
				}
				
				HitTile(pos);
			}
		}
		
		private void HitTile(Vector3Int target)
		{
			TileData td = _data[target];
			td.Hit();
			
			if(td.IsDestroyed())
			{
				_tilemap.SetTile(target, null);
				_data.Remove(target);
				
				PlaySound(td.RuleTile.DestroySound);
				return;
			}
			
			PlaySound(td.RuleTile.HitSound);
		}
		
		private void PlaySound(AudioClip sound)
		{
			MMSoundManagerSoundPlayEvent.Trigger(sound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
		}
		
		private bool DataContains(Vector3Int target)
		{
			return _data.ContainsKey(target);
		}
		
		private bool IsHittable(Vector3Int target)
		{
			return _tilemap.HasTile(target);
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using NavMeshPlus.Components;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
	public class DynamicTilemap : MonoBehaviour
	{
		[SerializeField]
		private TilemapObject _tmToOverride;
		[SerializeField]
		private MMF_Player _hitFeedbacks;
		[SerializeField]
		private bool _canMine = true;
		private Timer _restoreHpTimer;
		private Tilemap _tilemap;
		
		public Tilemap Tilemap => _tilemap;
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
			
			public void Hit(int amount = 1)
			{
				_currentHitPoints -= amount;
			}
			
			public void Restore()
			{
				if(RuleTile != null)
					_currentHitPoints = RuleTile.MaxHitPoints;
			}
			
			public bool IsDestroyed()
			{
				return _currentHitPoints <= 0;
			}
		}
		
		private void OnEnable()
		{
			_tmToOverride.DynamicTilemap = this;
			_tilemap = GetComponent<Tilemap>();
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			_tmToOverride.DynamicTilemap = this;
		}
		
		private void OnDestroy()
		{
			_restoreHpTimer.OnTimerEnd -= RestoreHitPoints;
		}
		
		private void Awake()
		{
			_restoreHpTimer = new(5);
			_restoreHpTimer.OnTimerEnd += RestoreHitPoints;
		}
		
		private void Update()
		{
			_restoreHpTimer.Tick(Time.deltaTime);
		}
		
		private void RestoreHitPoints()
		{
			foreach (KeyValuePair<Vector3Int, TileData> item in _data)
			{
				item.Value.Restore();
			}
			
			_data = new();
		}
		
		public void Hit(Vector3Int pos, ToolType toolType, int amount = 1)
		{
			if(IsHittable(pos))
			{
				if(!DataContains(pos))
				{
					RuleTileExtended tile = Tilemap.GetTile<RuleTileExtended>(pos);
					TileData td = new(tile);
					_data.Add(pos, td);
				}
				
				HitTile(pos, toolType, amount);
			}
		}
		
		public void HitTile(Vector3Int target, ToolType tooltype, int amount = 1)
		{
			_restoreHpTimer.RemainingSeconds = 5;
			TileData tileData = _data[target];
			
			if(tileData.RuleTile.HitToolType == tooltype)
			{
				tileData.Hit(amount);
				PlayGameFeel(target);
				PlaySound(tileData.RuleTile.HitSound);
				GameSignals.CLICKABLE_CLICKED.Dispatch();
				
				if(tileData.IsDestroyed())
				{
					Tilemap.SetTile(target, null);
					_data.Remove(target);
					_hitFeedbacks.transform.GetChild(1).gameObject.SetActive(false);
					PlaySound(tileData.RuleTile.DestroySound);
					SpawnItemFromTileData(tileData, target);
					AStarExtensions.Instance.UpdatePathfinding(target, new(4, 4, 4));
				}
			}
		}
		
		public void DestroyTile(Vector3Int target)
		{
			var tile = Tilemap.GetTile(target);
			
			if(tile is RuleTileExtended)
			{
				RuleTileExtended t = (RuleTileExtended)tile;
				Vector2 spawnPos = new(target.x + UnityEngine.Random.Range(0.25f, 0.75f), target.y + UnityEngine.Random.Range(0.25f, 0.75f));
				GameAssets.Instance.SpawnItem(spawnPos, t.Item, 1);
				Tilemap.SetTile(target, null);
				AStarExtensions.Instance.UpdatePathfinding(target, new(4, 4, 4));
			}
		}
		
		private void PlayGameFeel(Vector3Int target)
		{
			if(_hitFeedbacks == null) return;
			
			var sr = _hitFeedbacks.transform.GetChild(1).GetComponent<SpriteRenderer>();
			sr.sprite = Tilemap.GetSprite(target);
			_hitFeedbacks.gameObject.transform.position = new Vector3(target.x + 0.5f, target.y + 0.5f);
			_hitFeedbacks.PlayFeedbacks();
			
		}
		
		private void SpawnItemFromTileData(TileData data, Vector3Int target)
		{
			Vector2 spawnPos = new(target.x + UnityEngine.Random.Range(0.25f, 0.75f), target.y + UnityEngine.Random.Range(0.25f, 0.75f));
			GameAssets.Instance.SpawnItem(spawnPos, data.RuleTile.Item, 1);
		}
		
		private void PlaySound(AudioClip sound)
		{
			MMSoundManagerSoundPlayEvent.Trigger(sound, MMSoundManager.MMSoundManagerTracks.Sfx, default, pitch:UnityEngine.Random.Range(0.9f, 1.1f), volume:1.1f);
		}
		
		private bool DataContains(Vector3Int target)
		{
			return _data.ContainsKey(target);
		}
		
		private bool IsHittable(Vector3Int target)
		{
			return _canMine && Tilemap != null && Tilemap.HasTile(target);
		}
	}
}

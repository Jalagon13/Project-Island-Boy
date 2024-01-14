using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using NavMeshPlus.Components;
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
		[Tooltip("Navmesh to update on tile update")]
		[SerializeField]
		private NavMeshSurface _navMesh;
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
			
			public void Hit()
			{
				_currentHitPoints--;
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
		
		public void Hit(Vector3Int pos, ToolType toolType)
		{
			if(IsHittable(pos))
			{
				if(!DataContains(pos))
				{
					RuleTileExtended tile = Tilemap.GetTile<RuleTileExtended>(pos);
					TileData td = new(tile);
					_data.Add(pos, td);
				}
				
				HitTile(pos, toolType);
			}
		}
		
		private void HitTile(Vector3Int target, ToolType tooltype)
		{
			_restoreHpTimer.RemainingSeconds = 5;
			TileData tileData = _data[target];
			
			if(tileData.RuleTile.HitToolType == tooltype)
			{
				tileData.Hit();
				PlayGameFeel(target);
				GameSignals.CLICKABLE_CLICKED.Dispatch();
				
				if(tileData.IsDestroyed())
				{
					Tilemap.SetTile(target, null);
					_data.Remove(target);
					_hitFeedbacks.transform.GetChild(1).gameObject.SetActive(false);
					PlaySound(tileData.RuleTile.DestroySound);
					UpdateNavMesh();
					SpawnItemFromTileData(tileData, target);
					return;
				}
				
				PlaySound(tileData.RuleTile.HitSound);
			}
		}
		
		public void UpdateNavMesh()
		{
			if(_navMesh != null)
			{
				_navMesh.hideEditorLogs = true;
				_navMesh.UpdateNavMesh(_navMesh.navMeshData);
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
			Vector2 spawnPos = new(target.x + 0.5f, target.y + 0.5f);
			GameAssets.Instance.SpawnItem(spawnPos, data.RuleTile.Item, 1);
		}
		
		private void PlaySound(AudioClip sound)
		{
			MMSoundManagerSoundPlayEvent.Trigger(sound, MMSoundManager.MMSoundManagerTracks.Sfx, default, pitch:UnityEngine.Random.Range(0.9f, 1.1f));
		}
		
		private bool DataContains(Vector3Int target)
		{
			return _data.ContainsKey(target);
		}
		
		private bool IsHittable(Vector3Int target)
		{
			return Tilemap.HasTile(target);
		}
	}
}

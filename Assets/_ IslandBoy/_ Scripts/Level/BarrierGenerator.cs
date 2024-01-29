using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
	public class BarrierGenerator : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private TilemapObject _groundTilemap;
		[SerializeField] private TileBase _barrierTile;
		[SerializeField] private float _updateDistance;
		private static int _width;
		private static int _height;
		
		private Vector2 _lastUpdatePosition;
		private static Tilemap _barrierTilemap;
		
		private void Awake()
		{
			_barrierTilemap = GetComponent<Tilemap>();
			
			
		}
		
		private void Start()
		{
			// _lastUpdatePosition = _po.Position;
			// GenerateBarrier();
		}
		
		private void Update()
		{
			if(Vector2.Distance(_po.Position, _lastUpdatePosition) >= _updateDistance)
			{
				// RefreshBarriers();
			}
		}
		
		// public static void GenerateBarrier()
		// {
		// 	// _barrierTilemap.ClearAllTiles();
			
		// 	var playerPos = Vector3Int.FloorToInt(_po.Position);
			
		// 	int halfWidth = _width / 2;
		// 	int halfHeight = _height / 2;
			
		// 	for (int y = -halfHeight; y <= halfHeight; y++)
		// 	{
		// 		for (int x = -halfWidth; x <= halfWidth; x++)
		// 		{
		// 			Vector3Int currentCell = new Vector3Int(playerPos.x + x, playerPos.y + y, playerPos.z);
					
		// 			if(_barrierTilemap.HasTile(currentCell))
		// 				continue;
					
		// 			if(!_groundTilemap.Tilemap.HasTile(currentCell))
		// 			{
		// 				_barrierTilemap.SetTile(currentCell, _barrierTile);
		// 				// _positionHistory.Add(currentCell);
		// 				AStarExtensions.Instance.UpdatePathfinding(currentCell, new(1,1,1));
		// 			}
		// 		}
		// 	}
			
		// 	// foreach (var item in _positionHistory)
		// 	// {
		// 	// 	if(_barrierTilemap.HasTile)
		// 	// }
			
		// 	_lastUpdatePosition = _po.Position;
		// }
	}
}

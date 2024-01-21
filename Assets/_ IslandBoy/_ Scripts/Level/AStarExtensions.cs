using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
	public class AStarExtensions : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private TilemapObject _groundTilemap;
		[SerializeField] private TileBase _barrierTile;
		[SerializeField] private int _width;
		[SerializeField] private int _height;
		
		private Tilemap _barrierTilemap;
		private static AStarExtensions _instance;
		
		public static AStarExtensions Instance => _instance;
		
		private void Awake()
		{
			_barrierTilemap = transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
			_instance = this;
		}
		
		private void Start()
		{
			GenerateBarriers();
		}
		
		public void UpdatePathfinding(Vector3Int target, Vector3 size)
		{
			StartCoroutine(Delay(target.x, target.y, target.z, size));
		}
		
		public void UpdatePathfinding(Vector3 target, Vector3 size)
		{
			StartCoroutine(Delay(target.x, target.y, target.z, size));
		}
		
		private static IEnumerator Delay(float targetx, float targety, float targetz, Vector3 size)
		{
			yield return new WaitForSeconds(0.15f);
			
			Bounds bounds = new()
			{
				center = new(targetx, targety, targetz),
				size = size
			};
			var guo = new GraphUpdateObject(bounds);
			AstarPath.active.UpdateGraphs(guo);
		}
		
		public void GenerateBarriers()
		{
			_barrierTilemap.ClearAllTiles();
			
			var playerPos = Vector3Int.FloorToInt(_po.Position);
			int halfWidth = _width / 2;
			int halfHeight = _height / 2;
			
			for (int y = -halfHeight; y <= halfHeight; y++)
			{
				for (int x = -halfWidth; x <= halfWidth; x++)
				{
					Vector3Int currentCell = new Vector3Int(playerPos.x + x, playerPos.y + y, playerPos.z);
					
					if(_barrierTilemap.HasTile(currentCell))
						continue;
					
					if(!_groundTilemap.Tilemap.HasTile(currentCell))
					{
						_barrierTilemap.SetTile(currentCell, _barrierTile);
						UpdatePathfinding(currentCell, new(1,1,1));
					}
				}
			}
		}
	}
}

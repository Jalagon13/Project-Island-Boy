using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Pathfinding;
using UnityEngine;

namespace IslandBoy
{
	public class RangedMonsterStateManager : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private Ammo _launchPrefab;
		[SerializeField] private MMF_Player _spawnFeedback;
		[SerializeField] private float _shootCooldown;
		[SerializeField] private int _maxTeleportRange;
		
		private void Awake() 
		{
			_spawnFeedback?.PlayFeedbacks();
		}
		
		private IEnumerator Start() 
		{
			Teleport();
			
			yield return new WaitForSeconds(_shootCooldown);
			
			for (int i = 0; i < 4; i++)
			{
				ShootPlayer();
				yield return new WaitForSeconds(0.5f);
			}
			
			yield return new WaitForSeconds(_shootCooldown);
			
			StartCoroutine(Start());
		}
		
		private void ShootPlayer()
		{
			Ammo ammo = Instantiate(_launchPrefab, transform.position, Quaternion.identity);
			Vector3 direction = (_po.Position - (Vector2)ammo.transform.position).normalized;
			ammo.Setup(direction);
			
			// need visual indicator for mosnter spawning
		}
		
		private void Teleport()
		{
			transform.position = CalcTpPos();
		}
		
		private Vector2 CalcTpPos()
		{
			GraphNode startNode = AstarPath.active.GetNearest(_po.Position, NNConstraint.Default).node; 
 
			List<GraphNode> nodes = PathUtilities.BFS(startNode, _maxTeleportRange); 
			Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0]; 
 
			return singleRandomPoint; 
		}
	}
}

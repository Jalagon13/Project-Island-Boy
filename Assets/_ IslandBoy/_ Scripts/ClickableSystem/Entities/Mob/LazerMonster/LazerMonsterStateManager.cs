using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Pathfinding;
using UnityEngine;

namespace IslandBoy
{
	public class LazerMonsterStateManager : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private GameObject _laser;
		[SerializeField] private MMF_Player _spawnFeedback;
		[SerializeField] private float _delayFire;
		[SerializeField] private float _afterFireDelay;
		[SerializeField] private int _maxTeleportRange;
		
		private void Awake() 
		{
			_spawnFeedback?.PlayFeedbacks();
		}
		
		private IEnumerator Start() 
		{
			Teleport();
			
			yield return new WaitForSeconds(_delayFire);
			
			FireLaser();
			
			while(_laser.gameObject.activeInHierarchy)
			{
				yield return new WaitForEndOfFrame();
			}
			
			yield return new WaitForSeconds(_afterFireDelay);
			
			StartCoroutine(Start());
		}
		
		private void FireLaser()
		{
			_laser.gameObject.SetActive(true);
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

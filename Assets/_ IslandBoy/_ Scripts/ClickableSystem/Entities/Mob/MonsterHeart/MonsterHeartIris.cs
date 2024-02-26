using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class MonsterHeartIris : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		
		private void Update() 
		{
			// transform.right = (Vector3)_po.Position - transform.position;
			transform.SetPositionAndRotation(CalcPosition(), Quaternion.identity);
		}
		
		private Vector2 CalcPosition()
		{
			Vector2 taPosition;
			// Vector2 playerPos = transform.root.transform.localPosition + new Vector3(0, -0.3f, 0);
			Vector3 direction = ((Vector3)_po.Position + new Vector3(0, .2f) - transform.parent.parent.position).normalized;

			// taPosition = Vector2.Distance(transform.parent.parent.position, (Vector3)_po.Position) > 0.2 ? (transform.parent.parent.position += new Vector3(0, 0.25f)) + (direction * 0.2f) : (Vector3)_po.Position;
			taPosition = (transform.parent.parent.position + new Vector3(1, 0.95f)) + direction * 0.12f;
			return taPosition;
		}
	}
}

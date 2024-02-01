using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class TorchAI : MonoBehaviour
	{
		[SerializeField] private float _radius;
		
		// Method Start: Check for enemies to "light up"
		// OntriggerEnter if entity has light up effect execute it
		
		private void Start() 
		{
			RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _radius, default);
			
			foreach (RaycastHit2D hit in hits)
			{
				Entity entity = hit.collider.GetComponent<Entity>();
				
				if(entity != null)
				{
					// Burn enemies here
					SetEnemyOnFire(entity);
				}
			}
		}
		
		private void OnTriggerEnter2D(Collider2D other) 
		{
			Entity entity = other.GetComponent<Entity>();
			
			if(entity != null)
			{
				SetEnemyOnFire(entity);
			}
		}
		
		private void OnTriggerExit2D(Collider2D other) 
		{
			Entity entity = other.GetComponent<Entity>();
			
			if(entity != null)
			{
				
			}
		}
		
		private void SetEnemyOnFire(Entity entity)
		{
			
		}
	}
}

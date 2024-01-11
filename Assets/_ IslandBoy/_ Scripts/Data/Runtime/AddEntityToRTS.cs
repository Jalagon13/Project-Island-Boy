using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	[RequireComponent(typeof(Entity))]
	public class AddEntityToRTS : MonoBehaviour
	{
		[SerializeField] private EntityRuntimeSet _entityRTS;
		
		private Entity _entity;
		
		private void Awake()
		{
			_entity = GetComponent<Entity>();
		}
		
		private void OnEnable()
		{
			_entityRTS.AddToList(_entity);
		}
		
		private void OnDisable()
		{
			_entityRTS.RemoveFromList(_entity);
		}
	}
}

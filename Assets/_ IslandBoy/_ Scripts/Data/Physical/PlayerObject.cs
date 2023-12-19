using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
	[CreateAssetMenu(fileName = "New Player Reference", menuName = "New Reference/Player Reference")]
	public class PlayerObject : ScriptableObject
	{
		private int _defense; // set to 0 in PlayerEntity
		private Vector2 _spawnPoint;
		private Vector2 _playerPosition;
		private Vector2 _mousePosition;
		private Inventory _playerInventory;
		private GameObject _playerGameObject;

		/// <summary>
		/// Setting Position or MousePosition does NOT change the actual position or mouse position of the player.
		/// It just changes THESE local variables respectively that should ONLY be referenced.
		/// SpawnPoint, Position and MousePosition are set in PlayerStateMachine.
		/// </summary>
		public Vector2 Position { get { return _playerPosition; } set { _playerPosition = value; } }
		public Vector2 MousePosition { get { return _mousePosition; } set { _mousePosition = value; } }
		public Vector2 SpawnPoint { get { return _spawnPoint; } set { _spawnPoint = value; } }
		public GameObject GameObject {get {return _playerGameObject; } set { _playerGameObject = value; }}
		
		// This is only SET in the Inventory Script do NOT SET this anywhere else. This is for reference only.
		public Inventory Inventory { get { return _playerInventory; } set { _playerInventory = value; } }

		public int Defense { get { return _defense; } set { _defense = value; } }

		public void AddDefense(int val)
		{
			_defense += val;

			if(_defense < 0)
				_defense = 0;
		}
	}
}

using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
	[CreateAssetMenu(fileName = "New Player Reference", menuName = "New Reference/Player Reference")]
	public class PlayerObject : ScriptableObject
	{
		private int _defense;
		private Vector2 _spawnPoint;
		private Vector2 _playerPosition;
		private Vector2 _mousePosition;
		private Inventory _playerInventory;
		private GameObject _playerGameObject;
		private MouseSlot _playerMouseSlot;

		public Vector2 Position { get { return _playerPosition; } set { _playerPosition = value; } }
		public Vector2 MousePosition { get { return _mousePosition; } set { _mousePosition = value; } }
		public Vector2 SpawnPoint { get { return _spawnPoint; } set { _spawnPoint = value; } }
		public GameObject GameObject {get {return _playerGameObject; } set { _playerGameObject = value; }}
		public Inventory Inventory { get { return _playerInventory; } set { _playerInventory = value; } }
		public MouseSlot MouseSlot { get { return _playerMouseSlot; } set { _playerMouseSlot = value; } }

		public int Defense { get { return _defense; } set { _defense = value; } }

		public void AddDefense(int val)
		{
			_defense += val;

			if(_defense < 0)
				_defense = 0;
		}
	}
}

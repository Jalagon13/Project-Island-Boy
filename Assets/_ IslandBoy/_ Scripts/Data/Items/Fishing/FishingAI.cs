using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class FishingAI : MonoBehaviour
	{
		[SerializeField] private PlayerObject _pr; 
		[SerializeField] private FishingDatabaseObject _db;
		[SerializeField] private ItemObject _bait;
		[SerializeField] private Transform _target; //Target point to rotate around
		[SerializeField] private float _radius;
		[SerializeField] private float _fishingProgressSpeed; //how fast the fishing progress bar fills up
		private ItemObject _fish;

		// speed of the fish
		private float _maxSpeed;
		private float _minSpeed;
		private float _speed;

		// how long a fish moves in the current direction (in seconds)
		private float _maxDistance;
		private float _minDistance;
		private float _distance;
		private float _currentDistance;

		// how long a fish waits once it reaches the current direction, before choosing a new direction
		private float _maxWait;
		private float _minWait;
		private float _wait;
		private float _currentWait;

		private int[] directions = { -1, 1 };
		private int _direction;
		private Vector2 _initialPosition;

		[Header("Audio")]
		[SerializeField] private AudioClip _successSound;
		[SerializeField] private AudioClip _fleeSound;

		private bool _onFish = false; 
		private float _angle;
		[HideInInspector] public float fishProgress; //current value of fishprogress bar

		private void Awake()
		{
			SetupFish();
			NewDirection();
			
			GameSignals.HOTBAR_SLOT_UPDATED.AddListener(EndMinigame);
		}
		
		private void Start() 
		{
			_pr.Inventory.RemoveItem(_bait, 1);
		}
		
		private void OnDestroy()
		{
			GameSignals.HOTBAR_SLOT_UPDATED.RemoveListener(EndMinigame);
		}

		void FixedUpdate()
		{
			if(Vector2.Distance(_initialPosition, _pr.Position) > 2.5f)
			{
				Fail();
				return;
			}
			
			if (fishProgress >= 1f)
				CatchFish();
			else if (!_onFish && fishProgress > 0f)
				fishProgress -= _fishingProgressSpeed;
			else if (!_onFish && fishProgress <= 0f)
				Fail();

			if (_currentDistance < _distance)
				Move();
			else if (_currentWait < _wait)
				_currentWait += Time.deltaTime;
			else
				NewDirection();
		}

		void OnTriggerStay2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Fish")
			{
				_onFish = true;
				fishProgress += _fishingProgressSpeed;
			}
		}

		void OnTriggerExit2D(Collider2D collision)
		{
			_onFish = false;
		}

		void CatchFish()
		{
			_pr.Inventory.AddItem(_fish, 1);
			PopupMessage.Create(transform.position, $"You caught a {_fish.Name}!", Color.green, Vector2.up, 1f);
			MMSoundManagerSoundPlayEvent.Trigger(_successSound, MMSoundManager.MMSoundManagerTracks.Sfx, _target.transform.position);
			EndMinigame();
		}

		void Fail()
		{
			MMSoundManagerSoundPlayEvent.Trigger(_fleeSound, MMSoundManager.MMSoundManagerTracks.Sfx, _target.transform.position);
			EndMinigame();
		}

		void EndMinigame(ISignalParameters parameters = null)
		{
			Signal signal = GameSignals.FISHING_MINIGAME_END;
			signal.Dispatch();
			TakeBait();
			Destroy(_target.gameObject);
		}
		
		private void TakeBait()
		{
			
		}

		private void FixedMove() // old set movement system, doesn't change directions
		{
			float x = _target.position.x + Mathf.Cos(_angle) * _radius;
			float y = _target.position.y + Mathf.Sin(_angle) * _radius;

			transform.position = new Vector3(x, y, 0);

			_angle += 0.05f + Time.deltaTime;
		}

		private void Move()
		{
			float x = _target.position.x + Mathf.Cos(_angle) * _radius;
			float y = _target.position.y + Mathf.Sin(_angle) * _radius;
			transform.position = new Vector3(x, y, 0);
			
			_angle += (_speed + Time.deltaTime)*_direction;
			_currentDistance += Time.deltaTime;
		}

		private void NewDirection()
		{
			_direction = directions[UnityEngine.Random.Range(0, 2)]; 
			_speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);
			_wait = UnityEngine.Random.Range(_minWait, _maxWait);
			_distance = UnityEngine.Random.Range(_minDistance, _maxDistance);

			_currentDistance = 0f;
			_currentWait = 0f;
		}

		private void SetupFish()
		{
			// get randomized fish
			FishDifficulty difficulty = _db.Database[GetRandomFishIndex()];
			_fish = difficulty.fish;

			// set up difficulty
			_maxSpeed = difficulty.maxSpeed;
			_minSpeed = difficulty.minSpeed;
			_maxDistance = difficulty.maxDistance;
			_minDistance = difficulty.minDistance;
			_maxWait = difficulty.maxWait;
			_minWait = difficulty.minWait;
			
			_initialPosition = _pr.Position;
	}

		public int GetRandomFishIndex()
		{
			// total sum of all the weights
			float weightSum = 0f;
			for (int i = 0; i < _db.Database.Length; ++i)
			{
				weightSum += _db.Database[i].rarity;
			}

			// get randomized number
			float randNum = UnityEngine.Random.Range(0f, weightSum);
			float currentSum = 0f;
			float w;

			for (int i = 0; i < _db.Database.Length; ++i)
			{
				w = _db.Database[i].rarity;
				currentSum += w / weightSum;
				if (currentSum >= randNum) return i;
			}

			return -1;
		}
	}
}

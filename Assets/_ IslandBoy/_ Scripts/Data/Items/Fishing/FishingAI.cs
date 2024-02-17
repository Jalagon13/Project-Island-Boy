using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class FishingAI : MonoBehaviour
	{
		[SerializeField] private PlayerObject _pr; 
		[SerializeField] private ItemObject _fish;
		[SerializeField] private ItemObject _bait;
		[SerializeField] private Transform _target; //Target point to rotate around
		[SerializeField] private float _radius;
		[SerializeField] private float _fishingProgressSpeed; //how fast the fishing progress bar fills up

		[Header("Fish AI")]
		// speed of the fish
		[SerializeField] private float _maxSpeed = 0.05f;
		[SerializeField] private float _minSpeed = 0.01f;
		private float _speed;

		// how long a fish moves in the current direction (in seconds)
		[SerializeField] private float _maxDistance = 3f;
		[SerializeField] private float _minDistance = 0.5f;
		private float _distance;
		private float _currentDistance;

		// how long a fish waits once it reaches the current direction, before choosing a new direction
		[SerializeField] private float _maxWait = 1f;
		[SerializeField] private float _minWait = 0f;
		private float _wait;
		private float _currentWait;

		private int[] directions = { -1, 1 };
		private int _direction;

		[Header("Audio")]
		[SerializeField] private AudioClip _successSound;
		[SerializeField] private AudioClip _fleeSound;

		private bool _onFish = false; 
		private float _angle;
		[HideInInspector] public float fishProgress; //current value of fishprogress bar

		private void Awake()
		{
			NewDirection();
			
			
			GameSignals.HOTBAR_SLOT_UPDATED.AddListener(EndMinigame);
			
		}
		
		private void Start() 
		{
			Debug.Log("Taking biat");
			_pr.Inventory.RemoveItem(_bait, 1);
		}
		
		private void OnDestroy()
		{
			GameSignals.HOTBAR_SLOT_UPDATED.RemoveListener(EndMinigame);
		}

		void FixedUpdate()
		{
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
			PopupMessage.Create(transform.position, $"You caught a fish!", Color.green, Vector2.up, 1f);
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
	}
}

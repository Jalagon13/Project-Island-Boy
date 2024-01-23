using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class FishingHook : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _travelDistance;
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _minFishTime;
        [SerializeField] private float _maxFishTime;
        [SerializeField] private float _timeUntilFishLeaves;

        [Header("Tilemap")]
        [SerializeField] private TilemapObject _groundTm;

        [Header("Audio")]
        [SerializeField] private AudioClip _bubbleSound;
        [SerializeField] private AudioClip _fleeSound;
        [SerializeField] private AudioClip _minigameStartSound;

        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private GameObject _line;
        private Vector2 _targetPosition;
        private float currentTime = 0f;
        private float waitTime;
        private bool waitingForFish = false;
        private bool foundFish = false;

        private PlayerInput _catchInput;
        private GameObject _bubbles;
        [HideInInspector] public bool _inMinigame = false;

        [SerializeField] private GameObject _fishingUI;


        private void Awake()
        {
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _catchInput = new PlayerInput();
            _catchInput.Player.CatchFish.started += CatchFish;
            _catchInput.Enable();

            _bubbles = transform.GetChild(2).gameObject;

            _line = transform.GetChild(3).gameObject;
        }

        private void Start()
        {
            Vector2 direction = _rb.velocity.normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            _line.SetActive(true);
        }

        private void FixedUpdate()
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed);

            if (Vector2.Distance(transform.position, _targetPosition) < _maxDistance && !waitingForFish && !foundFish)
            {
                _speed = 0;
                if (ValidSpace())
                    WaitForFish();
                else
                    StopFishing();
            }

            if (waitingForFish && !foundFish)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= waitTime)
                    FishAppeared();
            }

            if (foundFish && !waitingForFish)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= _timeUntilFishLeaves)
                    FishDisappeared();
            }
        }

        public void Setup(ItemObject launchObject, Vector3 direction)
        {
            _targetPosition = transform.position + (direction * _travelDistance);
            RotateSpriteTowards(_targetPosition);
        }

        private void RotateSpriteTowards(Vector2 target)
        {
            Vector2 direction = (target - (Vector2)_sr.transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var offset = -135;
            _sr.transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }

        public bool ValidSpace()
        {
            var currentPos = Vector3Int.FloorToInt(transform.root.position);
            if (!_groundTm.Tilemap.HasTile(currentPos))
            {
                return true;
            }
            return false;
        }

        private void WaitForFish()
        {
            waitTime = UnityEngine.Random.Range(_minFishTime, _maxFishTime);
            waitingForFish = true;
        }
        
        private void FishAppeared()
        {
            foundFish = true;
            waitingForFish = false;
            currentTime = 0;

            _bubbles.SetActive(true);
            MMSoundManagerSoundPlayEvent.Trigger(_bubbleSound, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        }

        private void FishDisappeared()
        {
            foundFish = false;
            waitingForFish = true;
            currentTime = 0;

            _bubbles.SetActive(false);
            MMSoundManagerSoundPlayEvent.Trigger(_fleeSound, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            WaitForFish();
        }

        private void FishingMinigame()
        {
            waitingForFish = true;
            _catchInput.Disable();

            _bubbles.SetActive(false);
            MMSoundManagerSoundPlayEvent.Trigger(_minigameStartSound, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);

            _inMinigame = true;
            GameSignals.FISHING_MINIGAME_END.AddListener(FishingMinigameEnd);
            Instantiate(_fishingUI, transform.position, Quaternion.identity);
        }

        public void CatchFish(InputAction.CallbackContext context)
        {
            if (foundFish && !waitingForFish)
                FishingMinigame();
            else
                StopFishing();
        }

        void FishingMinigameEnd(ISignalParameters parameters)
        {
            GameSignals.FISHING_MINIGAME_END.RemoveListener(FishingMinigameEnd);
            StopFishing();
        }

        public void StopFishing()
        {
            _catchInput.Disable(); 
            if (gameObject != null)
            {
                _inMinigame = false;
                _catchInput.Disable();
                Destroy(gameObject);
            }
        }

        // MAYBE:
        // - add different fish types and difficulties
        // - switch keybinds from vb to ad and prevent player movement during minigame and maybe pause game while in fishing state
        // - maybe add rod power, like can cast rod farther out depending on how long you hold mouse
    }
}

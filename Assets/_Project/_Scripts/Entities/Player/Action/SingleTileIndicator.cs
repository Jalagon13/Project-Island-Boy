using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class SingleTileIndicator : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private AudioClip _lockOnSound;
        [SerializeField] private Tilemap _wallTilemap;
        [SerializeField] private Tilemap _floorTilemap;
        [SerializeField] private Color _indicatorEmptyColor;
        [SerializeField] private Color _indicatorTransparentColor;
        [SerializeField] private Color _canHitColor;

        private SingleTileHpCanvas _stHpCanvas;
        private SpriteRenderer _sr;

        public Tilemap WallTilemap { get { return _wallTilemap; } }
        public Tilemap FloorTilemap { get { return _floorTilemap; } }

        private void Awake()
        {
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _stHpCanvas = GetComponent<SingleTileHpCanvas>();
            //_wallTilemap = GameObject.FindGameObjectWithTag("Wall Tilemap").GetComponent<Tilemap>();
        }

        private void Update()
        {
            UpdateIndicator();

            if (_pr.SelectedSlot.ItemObject == null) return;

            if(_pr.SelectedSlot.ItemObject.ToolType == ToolType.Sword)
            {
                ChangeToOffIndicator();
            }
        }

        private void UpdateIndicator()
        {
            if (!transform.hasChanged) return;

            _stHpCanvas.HideHpCanvas();

            var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);
            int breakableCount = 0;
            IBreakable breakable = null;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IBreakable b))
                {
                    breakable = b;
                    breakableCount++;
                }
            }

            if (breakableCount > 0)
            {
                ChangeToOnIndicator();
                AudioManager.Instance.PlayClip(_lockOnSound, false, false, 0.35f);

                if (breakable == null)
                {
                    transform.hasChanged = false;
                    return;
                }

                if (breakable.CurrentHitPoints < breakable.MaxHitPoints)
                {
                    _stHpCanvas.ShowHpCanvas(breakable.MaxHitPoints, breakable.CurrentHitPoints);
                }
            }
            else
            {
                ChangeToOffIndicator();
            }

            if(_wallTilemap.HasTile(Vector3Int.FloorToInt(transform.position)) ||
               _floorTilemap.HasTile(Vector3Int.FloorToInt(transform.position)))
                ChangeToOnIndicator();

            transform.hasChanged = false;
        }

        public void ChangeToOnIndicator()
        {
            _sr.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            _sr.color = _canHitColor;
        }

        public void ChangeToOffIndicator()
        {
            _sr.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);

            if (_pr.SelectedSlot.ItemObject is DeployObject ||
                _pr.SelectedSlot.ItemObject is FloorObject ||
                _pr.SelectedSlot.ItemObject is WallObject)
                _sr.color = _indicatorTransparentColor;
            else
                _sr.color = _indicatorEmptyColor;


            _stHpCanvas.HideHpCanvas();
        }
    }
}

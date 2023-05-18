using UnityEngine;

namespace IslandBoy
{
    public class SingleTileIndicator : MonoBehaviour
    {
        [SerializeField] private Color _indicatorEmptyColor;
        [SerializeField] private Color _canHitColor;

        private SingleTileHpCanvas _stHpCanvas;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _stHpCanvas = GetComponent<SingleTileHpCanvas>();
        }

        private void Update()
        {
            if (transform.hasChanged)
            {
                _stHpCanvas.HideHpCanvas();

                var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);
                int breakableCount = 0;
                IBreakable breakable = null;

                foreach (var collider in colliders)
                {
                    if(collider.TryGetComponent(out IBreakable b))
                    {
                        breakable = b;
                        breakableCount++;
                    }
                }

                if (breakableCount > 0)
                {
                    ChangeToHit();
                    
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
                    ChangeToEmpty();
                }

                transform.hasChanged = false;
            }
        }

        public void ChangeToHit()
        {
            _sr.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            _sr.color = _canHitColor;
        }

        public void ChangeToEmpty()
        {
            _sr.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            _sr.color = _indicatorEmptyColor;

            _stHpCanvas.HideHpCanvas();
        }
    }
}

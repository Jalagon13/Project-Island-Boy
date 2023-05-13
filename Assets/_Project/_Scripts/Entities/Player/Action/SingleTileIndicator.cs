using UnityEngine;

namespace IslandBoy
{
    public class SingleTileIndicator : MonoBehaviour
    {
        [SerializeField] private Color _indicatorEmptyColor;
        [SerializeField] private Color _canHitColor;

        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>(); 
        }

        private void Update()
        {
            if (transform.hasChanged)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);
                int breakableCount = 0;

                foreach (var collider in colliders)
                {
                    IBreakable breakable = collider.GetComponent<IBreakable>();

                    if (breakable != null)
                    {
                        breakableCount++;
                    }
                }

                if (breakableCount > 0)
                    ChangeToHitColor();
                else
                    ChangeToEmptyColor();

                transform.hasChanged = false;
            }
        }

        public void ChangeToHitColor()
        {
            _sr.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            _sr.color = _canHitColor;
        }

        public void ChangeToEmptyColor()
        {
            _sr.transform.localScale = Vector3.one;
            _sr.color = _indicatorEmptyColor;
        }
    }
}

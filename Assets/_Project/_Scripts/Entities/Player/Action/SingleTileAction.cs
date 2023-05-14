using UnityEngine;

namespace IslandBoy
{
    public class SingleTileAction : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private int _basePower;

        public int BasePower { set { _basePower = value; } }

        private void Update()
        {
            transform.position = CalcStaPos();
        }

        public bool IsClear()
        {
            // need to flesh this out later
            // checks if there is empty space over the STA
            return true;
        }

        public void HitTile()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

            foreach (var collider in colliders)
            {
                IBreakable breakable = collider.GetComponent<IBreakable>();

                if (breakable != null)
                    breakable.Hit(_basePower);
            }
        }

        private Vector2 CalcStaPos()
        {
            var playerPosTileCenter = GetCenterOfTilePos(_pr.PositionReference);
            var dir = (_pr.MousePositionReference - playerPosTileCenter).normalized;

            return GetCenterOfTilePos(playerPosTileCenter + dir);
        }

        private Vector2 GetCenterOfTilePos(Vector3 pos)
        {
            var xPos = Mathf.FloorToInt(pos.x) + 0.5f;
            var yPos = Mathf.FloorToInt(pos.y) + 0.5f;

            return new Vector2(xPos, yPos);
        }
    }
}

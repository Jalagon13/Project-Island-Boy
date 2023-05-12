using UnityEngine;

namespace IslandBoy
{
    public class SingleTileAction : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private void Awake()
        {
            transform.SetParent(null);
        }

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

        private Vector2 CalcStaPos()
        {
            var playerPosTileCenter = GetCenterOfTilePos(_pr.PlayerPositionReference);
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

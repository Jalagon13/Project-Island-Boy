using UnityEngine;

namespace IslandBoy
{
    public class WorldItemTool : WorldItem
    {
        [SerializeField] private ToolObject _toolItem;

        private int _currentDurability;

        public void InitializeItemInWorld()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Collect"))
            {

            }
        }
    }
}

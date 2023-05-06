using UnityEngine;

namespace IslandBoy
{
    public class WorldItemResource : WorldItem
    {
        [SerializeField] private ResourceObject _resourceItem;

        private int _currentStack;

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

using UnityEngine;

namespace IslandBoy
{
    public class TreevilRoot : MonoBehaviour
    {
        [SerializeField] private int _damageAmount;
        [SerializeField] private float _waitLength; // in seconds, how long the root waits before attacking
        [SerializeField] private float _attackLength; // in seconds, how long this strike attack lasts
        [SerializeField] private GameObject rootSprite, warningSprite;

        private float timer;
        private bool isStriking;

        void Start()
        {
            rootSprite.SetActive(false);
        }

        void FixedUpdate()
        {
            timer += Time.deltaTime;

            // Maybe implement root appearing animation in future
            if (!isStriking)
            {
                if (timer > _waitLength)
                {
                    isStriking = true;
                    timer = 0;
                    rootSprite.SetActive(true);
                    warningSprite.SetActive(false);
                    gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
                }
            }
            else if (timer > _attackLength)
            {
                Destroy(gameObject); // delete once attack is finished
            }
            
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            // Doesn't start damaging player until root comes up from ground
            if (isStriking && collision.TryGetComponent(out Player player))
            {
                Vector2 damagerPosition = transform.root.gameObject.transform.position;
                player.Damage(_damageAmount, damagerPosition);
            }
        }

}
}

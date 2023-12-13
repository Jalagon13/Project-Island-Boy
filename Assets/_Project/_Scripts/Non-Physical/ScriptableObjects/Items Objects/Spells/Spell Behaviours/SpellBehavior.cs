using UnityEngine;
using System.Collections.Generic;
/*
 * This class is applies a spell effect to all objects
 * within a certain radius from the center. It will
 * apply the effect once to each object and delete
 * itself after a certain amount of time.
 * 
 * It is intended to be extended for different spells to be cast.
 * 
 * - Kevin Jung :)
 */

namespace IslandBoy
{
    public abstract class SpellBehavior : MonoBehaviour
    {
        private ParticleSystem m_MagicParticles;
        private AudioSource m_MagicAudio;
        protected float m_MaxDamage = 100f;
        protected float m_MaxLifeTime = 0.5f;
        protected float m_SplashRadius = 5f;

        private List<Collider2D> colliders;

        private void Awake()
        {
            colliders = new List<Collider2D>();
        }


        private void Start()
        {
            Destroy(gameObject, m_MaxLifeTime);
        }

        /* Prob dont need nevermind
        private void Update()
        {
            //Got to keep track of maxlife for multi -Nick
            if()
            m_MaxLifeTime -= 1;
            if(m_MaxLifeTime <= 0)

        }
        */


        private protected void OnTriggerEnter2D(Collider2D other)
        {
            if (!colliders.Contains(other)) // if the object has not been processed yet
            {
                colliders.Add(other);
                Debug.Log("Ow");

                ApplySpell(other.transform.position, other.GetComponent<ConsumableObject>());

                m_MagicParticles.transform.parent = null;

                m_MagicParticles.Play();

                //m_MagicAudio.Play();

                Destroy(m_MagicParticles.gameObject, m_MagicParticles.main.duration);
            }
        }

        // Abstract method for child classes to override
        public abstract void ApplySpell(Vector2 targetPosition, ConsumableObject playerHealth);
    }
}
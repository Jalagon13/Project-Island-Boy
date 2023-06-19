using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class GameAssets : MonoBehaviour
    {
        public Transform pfDamagePopup;

        private static GameAssets _i;

        public static GameAssets I
        {
            get
            {
                if (_i == null)
                    _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
                return _i;
            }
        }

        public ParticleSystem SpawnParticles(ParticleSystem particleSystem, Vector2 position)
        {
            var particles = Instantiate(particleSystem, position, particleSystem.transform.rotation);
            return particles;
        }

    }
}

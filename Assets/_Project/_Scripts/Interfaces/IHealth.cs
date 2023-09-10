using UnityEngine;

namespace IslandBoy
{
    public interface IHealth<T>
    {
        void Damage(T damageAmount, GameObject sender = null);
        void Heal(T healAmount);
        void KillEntity();
    }
}

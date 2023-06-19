namespace IslandBoy
{
    public interface IHealth<T>
    {
        void Damage(T damageAmount);
        void Heal(T healAmount);
        void OnDeath();
    }
}

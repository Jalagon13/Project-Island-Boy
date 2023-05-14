namespace IslandBoy
{
    public interface IBreakable
    {
        public int HitPoints { get; set; }

        public void Hit(int amount);

        public void Break();
    }
}

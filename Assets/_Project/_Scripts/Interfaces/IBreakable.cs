namespace IslandBoy
{
    public interface IBreakable
    {
        public int HitPoints { get; set; }

        public void Hit();

        public void Break();
    }
}

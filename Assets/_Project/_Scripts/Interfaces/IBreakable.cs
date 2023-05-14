namespace IslandBoy
{
    public interface IBreakable
    {
        public float HitPoints { get; set; }

        public void Hit(float amount, ToolType toolType);

        public void Break();
    }
}

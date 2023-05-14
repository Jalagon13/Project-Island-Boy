namespace IslandBoy
{
    public interface IBreakable
    {
        public float HitPoints { get; set; }

        public bool Hit(float amount, ToolType toolType);

        public void Break();
    }
}

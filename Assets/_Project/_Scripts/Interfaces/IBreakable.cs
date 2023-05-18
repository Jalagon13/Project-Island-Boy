namespace IslandBoy
{
    public interface IBreakable
    {
        public float MaxHitPoints { get; set; }
        public float CurrentHitPoints { get; set; }

        public bool Hit(float amount, ToolType toolType);

        public void Break();
    }
}

namespace IslandBoy
{
    public interface IBreakable
    {
        public float MaxHitPoints { get; set; }
        public float CurrentHitPoints { get; set; }
        public ToolType BreakType { get; set; }

        public void Hit(ToolType toolType);

        public void Break();
    }
}

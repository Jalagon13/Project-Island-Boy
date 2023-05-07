namespace IslandBoy
{
    public interface IInventoryItemInitializer
    {
        public ItemObject Item { get; }

        public void Initialize(ItemObject item);

    }
}

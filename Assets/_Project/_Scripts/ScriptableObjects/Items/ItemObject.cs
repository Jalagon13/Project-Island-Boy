using UnityEngine;

namespace IslandBoy
{
    public abstract class ItemObject : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _uiDisplay;
        [TextArea]
        [SerializeField] private string _description;

        public string Name { get { return _name; } }
        public Sprite UIDisplay { get { return _uiDisplay; } }
        public string Description { get { return _description; } }

    }
}

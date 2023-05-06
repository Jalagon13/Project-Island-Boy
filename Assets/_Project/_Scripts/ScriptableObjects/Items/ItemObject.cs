using UnityEngine;

namespace IslandBoy
{
    public abstract class ItemObject : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _uiDisplay;
        [TextArea]
        [SerializeField] private string _description;
        [SerializeField] private bool _stackable;

        public int ID { get { return _id; } }
        public string Name { get { return _name; } }
        public Sprite UIDisplay { get { return _uiDisplay; } }
        public string Description { get { return _description; } }
        public bool Stackable { get { return _stackable; } }
    }
}

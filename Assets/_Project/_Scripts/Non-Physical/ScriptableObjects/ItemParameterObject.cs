using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu]
    public class ItemParameterObject : ScriptableObject
    {
        [field: SerializeField] public string ParameterName { get; private set; }
    }
}

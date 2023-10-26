using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{

    [CreateAssetMenu(fileName = "[NPC] ", menuName = "New Npc")]
    public class NpcObject : ScriptableObject
    {
        public string Name;
        [field: TextArea]
        public string Description;
        public Sprite Icon;
        public GameObject NPC;
        public List<DeployObject> FurnitureCheckList;

        private bool _movedIn;
        private GameObject _worldEntity;

        public bool MovedIn { get { return _movedIn; } set { _movedIn = value; Debug.Log("move in set " + _movedIn); } }

        public void MoveInNpc(Bed homeBed)
        {
            if(_worldEntity != null)
                Destroy(_worldEntity);
            var pos = homeBed.gameObject.transform.position;
            Debug.Log($"Spawn pos for {Name}: {pos} Bed name: {homeBed.name}");
            _worldEntity = Instantiate(NPC, pos, Quaternion.identity);
        }

        public void MoveOutNpc()
        {
            if (_worldEntity != null)
                Destroy(_worldEntity);
            Debug.Log($"Moving out {Name}");
        }
    }
}

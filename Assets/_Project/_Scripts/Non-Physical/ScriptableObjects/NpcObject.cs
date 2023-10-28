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

        private bool _movedIn;
        private bool _discovered;
        private GameObject _worldEntity;
        private Bed _bed;

        public bool MovedIn { get { return _movedIn; } }
        public bool Discovered { get { return _discovered; } }
        public Bed Bed { get { return _bed; } }

        public void MoveIn(Bed homeBed)
        {
            if(_worldEntity != null)
                Destroy(_worldEntity);

            DispatchNpcMovedIn();

            var pos = homeBed.gameObject.transform.position;
            _worldEntity = Instantiate(NPC, pos, Quaternion.identity);
            _bed = homeBed;
            _movedIn = true;
        }

        private void DispatchNpcMovedIn()
        {
            Signal signal = GameSignals.NPC_MOVED_IN;
            signal.ClearParameters();
            signal.AddParameter("MovedInNpc", this);
            signal.Dispatch();
        }

        public void MoveOut()
        {
            if (_worldEntity != null)
                Destroy(_worldEntity);

            DispatchNpcMovedOut();

            _bed = null;
            _movedIn = false;
        }

        private void DispatchNpcMovedOut()
        {
            Signal signal = GameSignals.NPC_MOVED_OUT;
            signal.ClearParameters();
            signal.AddParameter("MovedOutNpc", this);
            signal.Dispatch();
        }
    }
}

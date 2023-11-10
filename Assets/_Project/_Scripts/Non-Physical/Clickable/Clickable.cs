using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public abstract class Clickable : MonoBehaviour
    {
        [SerializeField] protected int _maxHitPoints;
        [SerializeField] protected ToolType _breakType;

        protected int _currentHitPoints;

        public int MaxHitPoints { get { return _maxHitPoints; } set { _maxHitPoints = value; } }
        public int CurrentHitPoints { get { return _currentHitPoints; } set { _currentHitPoints = value; } }
        public ToolType BreakType { get { return _breakType; } set { _breakType = value; } }

        protected virtual void Awake()
        {
            _currentHitPoints = _maxHitPoints;
        }

        public virtual void OnClick(ToolType incomingToolType)
        {
            _currentHitPoints -= incomingToolType == _breakType ? _breakType == ToolType.None ? 1 : 2 : 1;

            GameSignals.CLICKABLE_CLICKED.Dispatch();

            if (_currentHitPoints <= 0)
                OnBreak();
        }

        protected abstract void OnBreak();
        public abstract void ShowDisplay();
        public abstract void HideDisplay();
    }
}

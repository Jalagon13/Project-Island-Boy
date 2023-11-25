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
        private Timer _timer;

        public int MaxHitPoints { get { return _maxHitPoints; } set { _maxHitPoints = value; } }
        public int CurrentHitPoints { get { return _currentHitPoints; } set { _currentHitPoints = value; } }
        public ToolType BreakType { get { return _breakType; } set { _breakType = value; } }

        protected virtual void Awake()
        {
            _timer = new(3f);
            _timer.RemainingSeconds = 0;
            _currentHitPoints = _maxHitPoints;
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("CursorControl")) return;
            ShowDisplay();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("CursorControl")) return;
            HideDisplay();
        }

        public virtual bool OnHit(ToolType incomingToolType, int amount, bool displayHit = true)
        {
            if(incomingToolType != _breakType || incomingToolType == ToolType.None)
            {
                if(displayHit && _timer.RemainingSeconds == 0)
                {
                    PopupMessage.Create(transform.position, $"I need a {_breakType} to hit this", Color.red, Vector2.one, 1f);
                    _timer.RemainingSeconds = 3;
                }

                return false;
            }

            _currentHitPoints -= amount;

            GameSignals.CLICKABLE_CLICKED.Dispatch();

            if (_currentHitPoints <= 0)
                OnBreak();

            return true;
        }

        protected virtual void OnBreak()
        {
            PlayerExperience.AddExerpience(_maxHitPoints);
            PopupMessage.Create(transform.position, $"+ {_maxHitPoints} XP", Color.white, Vector2.up, 1f);
        }

        public abstract void ShowDisplay();
        public abstract void HideDisplay();
    }
}

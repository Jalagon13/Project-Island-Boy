using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Experience
    {
        public event EventHandler OnCurrencyChanged;

        private int _count;

        public int Count { get { return _count; } }

        public void Add(int numToAdd)
        {
            _count += numToAdd;

            if(_count < 0)
                _count = 0;

            OnCurrencyChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Set(int numToSet)
        {
            _count = numToSet;

            OnCurrencyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

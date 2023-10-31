using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Currency
    {
        public event EventHandler OnCurrencyChanged;

        private int _count;

        public int Count { get { return _count; } }

        public void Add(int numToAdd)
        {
            int val = Mathf.Abs(numToAdd);

            _count += val;

            OnCurrencyChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Subtract(int numToSub)
        {
            int val = Mathf.Abs(numToSub);

            _count -= val;
            if (_count < 0)
                _count = 0;

            OnCurrencyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

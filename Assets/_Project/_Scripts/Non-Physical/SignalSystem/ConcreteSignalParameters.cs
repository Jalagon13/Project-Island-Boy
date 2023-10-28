using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class ConcreteSignalParameters : ISignalParameters
    {
        private Stack<Dictionary<string, object>> _parameterStack = new();

        public ConcreteSignalParameters()
        {
        }

        public void AddParameter(string key, object value)
        {
            _parameterStack.Peek()[key] = value;
        }

        public object GetParameter(string key)
        {
            return _parameterStack.Peek()[key];
        }

        public bool HasParameter(string key)
        {
            return _parameterStack.Peek().ContainsKey(key);
        }

        public void PushParameters()
        {
            _parameterStack.Push(NewParameterMap());
        }

        public void PopParameters()
        {
            _parameterStack.Pop();
        }

        public bool HasParameters
        {
            get
            {
                return _parameterStack.Count > 0;
            }
        }

        private static Dictionary<string, object> NewParameterMap()
        {
            Dictionary<string, object> newInstance = new();
            newInstance.Clear();
            return newInstance;
        }
    }
}

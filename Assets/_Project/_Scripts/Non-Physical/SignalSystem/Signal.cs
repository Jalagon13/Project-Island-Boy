using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Signal
    {
        private readonly string _name;
        private ConcreteSignalParameters _parameters;

        public delegate void SignalListener(ISignalParameters parameters);
        private List<SignalListener> _listenerList = new();

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public Signal(string name)
        {
            _name = name;
            _listenerList = new List<SignalListener>();
        }

        public void ClearParameters()
        {
            if (_parameters == null)
            {
                _parameters = new ConcreteSignalParameters();
            }

            _parameters.PushParameters();
        }

        public void AddParameter(string key, object value)
        {
            // This will throw an error if ClearParameters() is not invoked prior to calling this method
            _parameters.AddParameter(key, value);
        }

        public void AddListener(SignalListener listener)
        {
            _listenerList.Add(listener);
        }

        public void RemoveListener(SignalListener listener)
        {
            _listenerList.Remove(listener);
        }

        public void Dispatch()
        {
            try
            {
                if (_listenerList.Count == 0)
                {
                    Debug.LogWarning("There are no listeners to the signal: " + _name);
                }

                for (int i = 0; i < _listenerList.Count; ++i)
                {
                    // invoke the listeners
                    _listenerList[i](_parameters); // note that the parameters passed may be null if there was none specified
                }
            }
            finally
            {
                // Pop parameters for every Dispatch
                // We check if there was indeed parameters because there are signals that are dispatched without parameters
                if (_parameters != null && _parameters.HasParameters)
                {
                    _parameters.PopParameters();
                }
            }
        }
    }
}

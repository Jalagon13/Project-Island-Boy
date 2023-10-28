using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public interface ISignalParameters
    {
        void AddParameter(string key, object value);

        object GetParameter(string key);

        bool HasParameter(string key);
    }
}

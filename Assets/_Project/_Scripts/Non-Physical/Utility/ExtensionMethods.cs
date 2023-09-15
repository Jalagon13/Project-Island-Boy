using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public static class ExtensionMethods
    {
        public delegate void SpawnAction(Object obj);
        public static event SpawnAction OnSpawn;

        public static Object SpawnObject(Object obj, Vector3 pos, Quaternion rotation)
        {
            Object newObj = MonoBehaviour.Instantiate(obj, pos, rotation);

            if(OnSpawn != null)
                OnSpawn(newObj);

            return newObj;
        }
    }
}

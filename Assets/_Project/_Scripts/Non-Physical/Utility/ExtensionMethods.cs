using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IslandBoy
{
    public static class ExtensionMethods
    {
        //public delegate void SpawnAction(Object obj);
        //public static event SpawnAction OnSpawn;
        public static event EventHandler<Object> OnSpawnObject;

        public static Object SpawnObject(Object obj, Vector3 pos, Quaternion rotation)
        {
            Object newObj = MonoBehaviour.Instantiate(obj, pos, rotation);
            OnSpawnObject?.Invoke(default, newObj);
            //if (OnSpawn != null)
            //    OnSpawn(newObj);

            return newObj;
        }
    }
}

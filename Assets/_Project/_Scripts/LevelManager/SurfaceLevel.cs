using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SurfaceLevel : MonoBehaviour, IAppendToLevel
    {
        private GameObject _rscHolder;
        private GameObject _dplyHolder;
        private GameObject _wsHolder;

        private void Awake()
        {
            _rscHolder = transform.GetChild(0).gameObject;
            _dplyHolder = transform.GetChild(1).gameObject;
            _wsHolder = transform.GetChild(2).gameObject;
        }

        public void Append(GameObject obj)
        {
            switch (obj.tag)
            {
                case "RSC":
                    AppendToRSC(obj);
                    break;
                case "DPLY":
                    AppendToDPLY(obj);
                    break;
                case "WS":
                    AppendToWS(obj);
                    break;
                default:
                    Debug.LogError($"object {obj.name} could not be appended to Surface Level because TAG not found");
                    break;
            }
        }

        private void AppendToRSC(GameObject obj)
        {
            obj.transform.SetParent(_rscHolder.transform);
        }

        private void AppendToDPLY(GameObject obj)
        {
            obj.transform.SetParent(_dplyHolder.transform);
        }

        private void AppendToWS(GameObject obj)
        {
            obj.transform.SetParent(_wsHolder.transform);
        }
    }
}

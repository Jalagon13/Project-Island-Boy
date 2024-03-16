using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class StartupWait : MonoBehaviour
    {
        void Start()
        {
            GameSignals.SCENE_FINISH_SETUP.AddListener(EnableUI);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameSignals.SCENE_FINISH_SETUP.RemoveListener(EnableUI);
        }

        private void EnableUI(ISignalParameters parameters)
        {
            gameObject.SetActive(true);
        }
    }
}

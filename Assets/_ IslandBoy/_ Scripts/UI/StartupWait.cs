using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class StartupWait : MonoBehaviour
    {
        void Start()
        {
            GameSignals.SCENE_FINISH_SETUP.AddListener(EnableUI);
            // if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Surface"))
            //     gameObject.SetActive(false);
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

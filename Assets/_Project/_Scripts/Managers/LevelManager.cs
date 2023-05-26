using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private GameObject _loaderCanvas;

        public void LoadScene(string sceneName)
        {
            StartCoroutine(SceneLoading(sceneName));
        }

        private IEnumerator SceneLoading(string sceneName)
        {
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;

            do
            {
                yield return new WaitForSeconds(0.1f);
            }
            while (scene.progress < 0.9f);

            scene.allowSceneActivation = true;
        }
    }
}

using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class IslandShrine : MonoBehaviour
    {
        [SerializeField] private PlayerObject _pr;
        [SerializeField] private CraftingRecipeObject _expansionRecipe;
        [SerializeField] private int _unlockFee;
        [SerializeField] private string _transitionScene;
        [SerializeField] private TextMeshProUGUI _expansionText;
        [SerializeField] private AudioClip _expandSound;

        private void Awake()
        {
            string needList = string.Empty;

            foreach (ItemAmount itemAmount in _expansionRecipe.ResourceList)
            {
                needList += $"<br>* {itemAmount.Item.Name} ({itemAmount.Amount})";
            }

            needList += $"<br>* {_unlockFee} XP";

            _expansionText.text = $"Expand Island Reqs:{needList}";
        }

        public void TryExpand()
        {
            foreach (ItemAmount ia in _expansionRecipe.ResourceList)
            {
                bool canCraft = _pr.Inventory.Contains(ia.Item, ia.Amount);

                if (!canCraft)
                {
                    PopupMessage.Create(transform.position, $"I am missing some items", Color.yellow, Vector2.up * 2, 1f);
                    return;
                }
            }

            // if (PlayerExperience.Experience.Count < _unlockFee)
            // {
            //     PopupMessage.Create(transform.position, $"I need more XP", Color.yellow, Vector2.up * 2, 1f);
            //     return;
            // }

            foreach (ItemAmount ia in _expansionRecipe.ResourceList)
            {
                _pr.Inventory.RemoveItem(ia.Item, ia.Amount);
            }

            // PlayerExperience.AddExerpience(-_unlockFee);
            MMSoundManagerSoundPlayEvent.Trigger(_expandSound, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position, persistent:true);
            StartCoroutine(LoadSceneAdd(_transitionScene));
        }

        private IEnumerator LoadSceneAdd(string sceneName)
        {
            AsyncOperation sceneAsync = new();
            sceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            sceneAsync.allowSceneActivation = false;

            while (sceneAsync.progress < 0.9f)
            {
                yield return null;
            }

            sceneAsync.allowSceneActivation = true;

            while (!sceneAsync.isDone)
            {
                yield return null;
            }

            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(currentScene);
            Scene s = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(s);
        }

        //// super temporary
        //public void EndGame()
        //{
        //    // Transition to end scene.
        //    SceneManager.LoadScene("End");
        //}
    }
}

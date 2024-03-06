using UnityEngine;

namespace IslandBoy
{
    public class TreevilVanquishListener : MonoBehaviour
    {
        private void Awake()
        {
            GameSignals.TREEVIL_VANQUISHED.AddListener(TreevilVictory);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameSignals.TREEVIL_VANQUISHED.RemoveListener(TreevilVictory);
        }

        private void TreevilVictory(ISignalParameters parameters)
        {
            gameObject.SetActive(true);
        }
    }
}

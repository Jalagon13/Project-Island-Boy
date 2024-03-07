using UnityEngine;

namespace IslandBoy
{
    public class TreevilVanquishListener : MonoBehaviour
    {
        private void Awake()
        {
            GameSignals.TREEVIL_VANQUISHED.AddListener(TreevilVictory);
        }

        private void OnDestroy()
        {
            GameSignals.TREEVIL_VANQUISHED.RemoveListener(TreevilVictory);
        }

        private void TreevilVictory(ISignalParameters parameters)
        {
            gameObject.SetActive(false);
        }
    }
}

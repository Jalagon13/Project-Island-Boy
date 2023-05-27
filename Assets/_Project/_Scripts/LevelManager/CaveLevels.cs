using UnityEngine;

namespace IslandBoy
{
    public class CaveLevels : MonoBehaviour, IAppendToLevel
    {
        private int _currentLevelIndex;

        private void OnEnable()
        {
            DisableAllLevels();
            _currentLevelIndex = 0;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        public void Descend()
        {
            DisableAllLevels();
            _currentLevelIndex++;
            transform.GetChild(_currentLevelIndex).gameObject.SetActive(true);
        }

        public void Ascend()
        {
            DisableAllLevels();
            _currentLevelIndex--;
            transform.GetChild(_currentLevelIndex).gameObject.SetActive(true);
        }

        private void DisableAllLevels()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void Append(GameObject obj)
        {
            
        }
    }
}

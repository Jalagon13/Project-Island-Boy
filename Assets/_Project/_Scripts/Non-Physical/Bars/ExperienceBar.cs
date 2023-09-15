using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class ExperienceBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private AudioClip _expGainSound;
        [SerializeField] private AudioClip _levelUpSound;

        private LevelSystem _levelSystem;
        private Image _fillImage;

        private void Awake()
        {
            _fillImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        }

        public void TESTAddExp()
        {
            _levelSystem.AddExperience(500);
        }

        public void SetLevelSystem(LevelSystem ls)
        {
            // set the level system
            _levelSystem = ls;

            // update the starting values
            SetExperienceBarSize(_levelSystem.ExperienceNormalized);
            SetLevelText(_levelSystem.LevelNumber);

            // subscribe to the changed events
            _levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
            _levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
        }

        private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
        {
            SetLevelText(_levelSystem.LevelNumber);

            AudioManager.Instance.PlayClip(_levelUpSound, false, true);
        }

        private void LevelSystem_OnExperienceChanged(object sender, int e)
        {
            SetExperienceBarSize(_levelSystem.ExperienceNormalized);

            AudioManager.Instance.PlayClip(_expGainSound, false, true);
        }

        private void SetExperienceBarSize(float experienceNormalized)
        {
            _fillImage.fillAmount = experienceNormalized;
        }

        private void SetLevelText(int level)
        {
            _levelText.text = $"{level}" /*+ $"DIFF: {_levelSystem.ExperienceToNextLevel}"*/;
        }
    }
}

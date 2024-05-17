using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class SkillCategoryPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _skillNameText;
        [SerializeField] private TextMeshProUGUI _storedExpText;
        [SerializeField] private TextMeshProUGUI _multText;
        [SerializeField] private TextMeshProUGUI _convertedExpText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Transform _equationTransform;
        [SerializeField] private Image _fillImage;
        [SerializeField] private MMF_Player _appearFeedback;
        [SerializeField] private MMF_Player _expFeedback;
        [SerializeField] private MMF_Player _levelUpFeedback;
        private LevelSystem _levelSystem;
        private Category _category;
        private int _convertedExp, _currentLevel;
        
        public void Initialize(Category category, float multiplier)
        {
            string skillName = category.Skill.ToString();
            _category = category;
            _levelSystem = category.LevelSystem;
            _convertedExp = Mathf.RoundToInt(_category.StoredExp * multiplier);
            _currentLevel = _category.LevelSystem.CurrentLevel;
            
            UpdateTime();
            
            _skillNameText.text = $"{skillName}";
            _storedExpText.text = $"{_category.StoredExp}xp";
            _multText.text = $"x{multiplier}";
            _convertedExpText.text = $"+{_convertedExp}xp";
            _levelText.text = $"Lv.{_currentLevel}";
            _equationTransform.gameObject.SetActive(_category.StoredExp > 0);

            _appearFeedback?.PlayFeedbacks();
            
            if(_category.StoredExp > 0)
            {
                StartCoroutine(ExpSequence());
            }
        }
        
        private IEnumerator ExpSequence()
        {
            yield return new WaitForSeconds(2f);

            // make equation appear
            _storedExpText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _multText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _convertedExpText.gameObject.SetActive(true);

            yield return new WaitForSeconds(2f);


            int displayValue = _convertedExp - 1;
            for (int i = 0; i < _convertedExp; i++)
            {
                _levelSystem.GainExperience(1);
                // Debug.Log(_category.Skill + " " + _levelSystem.CurrentExp);
                if (_levelSystem.CurrentLevel > _currentLevel) // level up!
                {
                    _currentLevel = _levelSystem.CurrentLevel;
                    _levelText.text = $"Lv.{_currentLevel}";
                    _levelUpFeedback?.PlayFeedbacks();
                }
                else 
                    _expFeedback?.PlayFeedbacks();

                _convertedExpText.text = $"+{displayValue - i}xp";

                UpdateTime();
                
                yield return new WaitForSeconds(0.4f);
            }
            
            _category.StoredExp = 0;
        }
        
        public void UpdateTime()
        {
            int currentExp = _levelSystem.CurrentExp;
            
            int currentLevelAmount = _levelSystem.ExpPerLevel[_currentLevel];
            int nextLevelAmount = _levelSystem.ExpPerLevel[_currentLevel + 1];
            
            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, nextLevelAmount - currentLevelAmount, currentExp - currentLevelAmount));
        }
    }
}

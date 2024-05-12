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
        private LevelSystem _levelSystem;
        private Category _category;
        private int _convertedExp;
        
        public void Initialize(Category category, float multiplier)
        {
            string skillName = category.Skill.ToString();
            _category = category;
            _levelSystem = category.LevelSystem;
            _convertedExp = Mathf.RoundToInt(_category.StoredExp * multiplier);
            
            UpdateTime();
            
            _skillNameText.text = $"{skillName}";
            _storedExpText.text = $"{_category.StoredExp}xp";
            _multText.text = $"x{multiplier}";
            _convertedExpText.text = $"+{_convertedExp}xp";
            _levelText.text = $"Lv.{category.LevelSystem.CurrentLevel}";
            _equationTransform.gameObject.SetActive(_category.StoredExp > 0);
            
            if(_category.StoredExp > 0)
            {
                StartCoroutine(ExpSequence());
            }
        }
        
        private IEnumerator ExpSequence()
        {
            yield return new WaitForSeconds(3f);
            
            int displayValue = _convertedExp - 1;
            for (int i = 0; i < _convertedExp; i++)
            {
                _levelSystem.GainExperience(1);
                // Debug.Log(_category.Skill + " " + _levelSystem.CurrentExp);
                UpdateTime();
                _convertedExpText.text = $"+{displayValue - i}xp";
                _levelText.text = $"Lv.{_category.LevelSystem.CurrentLevel}";
                
                yield return new WaitForSeconds(0.2f);
            }
            
            _category.StoredExp = 0;
        }
        
        public void UpdateTime()
        {
            int currentLevel = _levelSystem.CurrentLevel;
            int currentExp = _levelSystem.CurrentExp;
            
            int currentLevelAmount = _levelSystem.ExpPerLevel[currentLevel];
            int nextLevelAmount = _levelSystem.ExpPerLevel[currentLevel + 1];
            
            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, nextLevelAmount - currentLevelAmount, currentExp - currentLevelAmount));
        }
    }
}

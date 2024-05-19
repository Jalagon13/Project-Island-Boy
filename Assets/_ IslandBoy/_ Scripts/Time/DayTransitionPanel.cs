using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class DayTransitionPanel : MonoBehaviour
    {
        [SerializeField] private PlayerObject _po;
        [SerializeField] private TimeView _timeView;
        [SerializeField] private RectTransform _skillCategoryHolder;
        [SerializeField] private SkillCategoryPanel _skillCategoryPanel;	
        
        private void OnEnable()
        {
            StartCoroutine(EndOfDaySequence());
        }
        
        private IEnumerator EndOfDaySequence()
        {
            // Depopulate SkillCategoryHolder
            foreach (Transform child in _skillCategoryHolder)
            {
                Destroy(child.gameObject);
            }
            
            yield return new WaitForSeconds(0.5f);
            
            // Populate SkillCategoryHolder
                // For each Skill Category Panel, Populate it with correct information, increment xp and display the incrementation.
                
            List<Category> skillCategories = _po.PlayerExperience.ExperienceModel.SkillCategories; 
            float multiplier = _po.PlayerExperience.ExperienceModel.ExpMultiplier;
            foreach (Category category in skillCategories)
            {
                if(category.Skill == SkillCategory.None) continue;
                
                yield return new WaitForSeconds(0.2f);
                
                var scp = Instantiate(_skillCategoryPanel, _skillCategoryHolder);
                scp.Initialize(category, multiplier);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class ExperienceView : MonoBehaviour
    {
        [SerializeField] private ExperienceNotifView _notification;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        
        public void UpdateMultiplierView(float multiplierToView)
        {
            _multiplierText.text = $"Multiplier: {multiplierToView}x";
        }
        
        public void NotifySkillExpGain(SkillCategory skill)
        {
            ExperienceNotifView expNotifView = Instantiate(_notification, transform.position, Quaternion.identity);
            expNotifView.transform.SetParent(transform);
            expNotifView.Initialize(skill);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class ExperienceNotifView : MonoBehaviour
    {
        [SerializeField] private MMF_Player _expFeedbacks;
        [SerializeField] private TextMeshProUGUI _text;
        
        public void Initialize(SkillCategory skill, int expAmount)
        {
            _text.text = $"+{expAmount} {skill} EXP";
            _expFeedbacks?.PlayFeedbacks();
        }
        
        public void OnFeedbackEnd()
        {
            Destroy(gameObject);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class LevelSystem
    {
        public event EventHandler OnLevelChanged;
        public event EventHandler OnExperienceChanged;

        private AnimationCurve _experienceCurve;
        private int _level;
        private int _experience;
        private int _totalExperience;
        private int _experienceToNextLevel;

        public int LevelNumber { get { return _level; } }
        public int ExperienceToNextLevel { get { return _experienceToNextLevel - (int)_experienceCurve.Evaluate(_level - 1); } }
        public float ExperienceNormalized { get { return (float)_experience / _experienceToNextLevel; } }

        public LevelSystem(AnimationCurve experienceCurve)
        {
            _level = 0;
            _experience = 0;
            _totalExperience = 0;
            _experienceCurve = experienceCurve;
            _experienceToNextLevel = (int)_experienceCurve.Evaluate(1);
        }

        public void AddExperience(int amount)
        {
            _experience += amount;
            _totalExperience += amount;
            
            if(_experience >= _experienceToNextLevel)
            {
                _level++;
                _experience -= _experienceToNextLevel;
                _experienceToNextLevel = (int)_experienceCurve.Evaluate(_level);
                OnLevelChanged?.Invoke(this, EventArgs.Empty);
            }

            OnExperienceChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

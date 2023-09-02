using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerExperience : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private ExperienceBar _experienceBar;
        [SerializeField] private AnimationCurve _levelCurve;

        private LevelSystem _playerLevelSystem;

        private void Start()
        {
            _playerLevelSystem = new(_levelCurve);
            _pr.SetPlayerLevelSystem(_playerLevelSystem);
            _experienceBar.SetLevelSystem(_playerLevelSystem);
        }
    }
}

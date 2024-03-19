using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class NPCSkin : MonoBehaviour
    {
        [SerializeField] private string _defaultSkinPrefix;
        [SerializeField] private Skins[] _skins;
        [SerializeField] private int _npcSkinNumber;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            ChooseSkin();
        }

        public void ChooseSkin()
        {
            if (_sr.sprite.name.Contains(_defaultSkinPrefix) && _npcSkinNumber != 0)
            {
                if (_sr.sprite.name.Contains("bound"))
                {
                    _sr.sprite = _skins[_npcSkinNumber].sprites[^1];
                }
                else
                {
                    string spriteName = _sr.sprite.name;
                    spriteName = spriteName.Replace(_defaultSkinPrefix, "");
                    int spriteNum = int.Parse(spriteName.Split('_')[^1]) - 1;
                    _sr.sprite = _skins[_npcSkinNumber].sprites[spriteNum];
                }
            }
        }
    }
}

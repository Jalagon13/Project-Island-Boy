using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [System.Serializable]
    public struct Skins
    {
        public Sprite[] sprites;
    }
    
    public class CustomizablePlayer : MonoBehaviour
    {
        [SerializeField] private PlayerObject _pr;
        [SerializeField] private string _defaultSkinPrefix;
        [SerializeField] private Skins[] _skins;
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
            if (_sr.sprite.name.Contains(_defaultSkinPrefix) && _pr.Skin != 0)
            {
                string spriteName = _sr.sprite.name;
                spriteName = spriteName.Replace(_defaultSkinPrefix, "");
                int spriteNum = int.Parse(spriteName.Split('_')[^1])-1;
                _sr.sprite = _skins[_pr.Skin].sprites[spriteNum];
            }
        }
    }
}

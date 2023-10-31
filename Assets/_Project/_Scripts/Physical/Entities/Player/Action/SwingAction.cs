using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SwingAction : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _sr.sprite = _pr.SelectedSlot.ItemObject != null ? _pr.SelectedSlot.ItemObject.UiDisplay : null;
        }
    }
}

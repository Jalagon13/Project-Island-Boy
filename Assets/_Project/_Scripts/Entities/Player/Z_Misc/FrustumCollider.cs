using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class FrustumCollider : MonoBehaviour
    {
        private Camera _mainCam;
        private BoxCollider2D _frustumCollider;

        private void Awake()
        {
            _mainCam = Camera.main;
            _frustumCollider = GetComponent<BoxCollider2D>();
        }

        private void LateUpdate()
        {
            float aspect = (float)Screen.width / Screen.height;
            _frustumCollider.size = new(_mainCam.orthographicSize * 2 * aspect, _mainCam.orthographicSize * 2);
        }
    }
}

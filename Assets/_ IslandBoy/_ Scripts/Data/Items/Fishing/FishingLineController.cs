using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class FishingLineController : MonoBehaviour
    {
        private LineRenderer _lr; 
        [SerializeField] private PlayerObject _pr;
        [SerializeField] private Transform _hookPoint;
        
        void Awake()
        {
            _lr = GetComponent<LineRenderer>();
            _lr.positionCount = 2;
        }

        public void SetUpHook(Transform point)
        {
            _hookPoint = point;
        }

        private void FixedUpdate()
        {
            _lr.SetPosition(0, _pr.Position + new Vector2(0, 0.5f));
            _lr.SetPosition(1, _hookPoint.position);
        }
    }
}

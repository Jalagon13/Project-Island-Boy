using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class FishingProgress : MonoBehaviour
    {
        [SerializeField] private FishingAI fish;
        private float fishProgress;
        Image fill;

        private void Awake()
        {
            fill = transform.GetChild(1).GetComponent<Image>();
        }
        void FixedUpdate()
        {
            fishProgress = fish.fishProgress;
            fill.fillAmount = fishProgress;
        }
    }
}

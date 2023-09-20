using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace IslandBoy
{
    public class DynamicLight : MonoBehaviour
    {
        private Light2D _light;
        private float _intensity;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _light.enabled = true;
            _intensity = _light.intensity;
        }

        private void LateUpdate()
        {
            if (!DayNightManager.Instance.GlobalVolume.isActiveAndEnabled)
            {
                _light.intensity = _intensity * 0.25f;
                return;
            }

            float globalBrightness = DayNightManager.Instance.GlobalVolume.weight;
            float intensity = _intensity * globalBrightness;

            _light.enabled = intensity > 1f;

            if (_light.enabled)
                _light.intensity = intensity;
        }
    }
}

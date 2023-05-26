using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class CaveCanvas : MonoBehaviour
    {
        private TextMeshProUGUI _floorText;

        public void Initialize()
        {
            _floorText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        public void UpdateFloorText(int level)
        {
            _floorText.text = $"Floor {level}";
        }
    }
}

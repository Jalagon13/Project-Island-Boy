using System.Collections;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class PickupSign : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TextMeshProUGUI _text;

        private static float _spawnHeight = 2.5f;
        private ItemObject _item;
        private int _amount;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(8);
            Destroy(gameObject);
        }

        public void Initialize(string text, Color textColor)
        {
            _text.text = text;
            _text.color = textColor;

            transform.position = _pr.Position + new Vector2(0f, _spawnHeight);
        }

        public void Initialize(ItemObject item, int amount, Color textColor)
        {
            _item = item;
            _amount = amount;
            _text.text = $"+{amount} {item.Name} ";
            _text.color = textColor;

            transform.position = _pr.Position + new Vector2(0f, _spawnHeight);
        }
    }
}

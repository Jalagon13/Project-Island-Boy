using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] int _maxHealth;
        [SerializeField] Sprite _filledHeartImage;
        [SerializeField] Sprite _emptyHeartImage;

        int currentHealth;

        private void Awake()
        {
            //UpdateMaxHealth(_maxHealth);
            currentHealth = _maxHealth;
        }


        public void UpdateHealth(int newHealth)
        {
            if (newHealth > _maxHealth)
                newHealth = _maxHealth;

            if (currentHealth < newHealth) // heal
            {
                for (int i = currentHealth; i < newHealth; ++i)
                    transform.GetChild(i).GetComponent<Image>().sprite = _filledHeartImage;
            }
            else if (currentHealth > newHealth) // damage
            {
                for (int i = currentHealth; i > newHealth; --i)
                    transform.GetChild(i - 1).GetComponent<Image>().sprite = _emptyHeartImage;
            }
            currentHealth = newHealth;
        }

        private void UpdateMaxHealth(int newMax)
        {
            if(transform.childCount < newMax)
            {
                while(transform.childCount < newMax)
                {
                    // add more hearts to UI
                }
            }
            else if(transform.childCount > newMax)
            {
                while (transform.childCount > newMax)
                {
                    // delete hearts from UI
                }
            }
            _maxHealth = newMax;
        }
    }
}

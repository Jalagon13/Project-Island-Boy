using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class DecreaseBtn : MonoBehaviour
    {
        public void EnableButton()
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            transform.GetComponent<Button>().enabled = true;
        }

        public void DisableButton()
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            transform.GetComponent<Button>().enabled = false;
        }
    }
}

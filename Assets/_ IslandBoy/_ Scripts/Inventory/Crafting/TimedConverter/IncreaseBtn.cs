using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class IncreaseBtn : MonoBehaviour
    {
        [SerializeField] private AudioClip _pressSound;

        public void ButtonGameFeel()
        {
            MMSoundManagerSoundPlayEvent.Trigger(_pressSound, MMSoundManager.MMSoundManagerTracks.UI, default, pitch: Random.Range(0.9f, 1.1f));
        }

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

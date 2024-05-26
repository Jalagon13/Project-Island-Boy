using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class TransitionScreen : MonoBehaviour
    {
        void Start()
        {
            transform.GetComponent<Image>().color = Color.black;
        }
    }
}

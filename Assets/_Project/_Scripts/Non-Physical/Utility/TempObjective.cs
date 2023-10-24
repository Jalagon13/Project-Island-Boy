using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    // temporary for prototype
    public class TempObjective : Interactable
    {
        [SerializeField] private List<ChestInvSlot> _itemsToCollect = new();

        public override void Interact()
        {
            bool toEndScreen = true;

            _itemsToCollect.ForEach(item => 
            {
                if (!_pr.Inventory.Contains(item.OutputItem, item.OutputAmount))
                    toEndScreen = false;
            });

            if (toEndScreen)
                SceneManager.LoadScene(3);
        }
    }
}

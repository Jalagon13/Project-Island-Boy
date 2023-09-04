using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[ADB] ", menuName = "New Augment Database")]
    public class AugmentDatabaseObject : ScriptableObject
    {
        public AugmentRecipe[] Database;
    }

    [Serializable]
    public class AugmentRecipe
    {
        public string Name;
        [field: TextArea]
        public string Description;
        public int LevelsRequired;
        public Sprite RecipeSprite;
        public List<AugmentObject> AugmentOutputs;
    }
}

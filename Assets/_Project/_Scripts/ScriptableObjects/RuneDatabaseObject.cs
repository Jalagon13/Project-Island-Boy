using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[RDB] ", menuName = "New Rune Database")]
    public class RuneDatabaseObject : ScriptableObject
    {
        public RuneRecipe[] Database;
    }

    [Serializable]
    public class RuneRecipe
    {
        public string Name;
        [field: TextArea]
        public string Description;
        public int LevelsRequired;
        public Sprite RecipeSprite;
        public List<RuneObject> AugmentOutputs;
    }
}

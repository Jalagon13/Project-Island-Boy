using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "[FISH] ", menuName = "Create Fish Difficulty")]
    public class FishDifficulty : ScriptableObject
    {
        public ItemObject fish;
        public float rarity;

        [Header("Fish AI")]
        // speed of the fish
        public float maxSpeed = 0.01f;
        public float minSpeed = 0.005f;

        // how long a fish moves in the current direction (in seconds)
        public float maxDistance = 3f;
        public float minDistance = 0.5f;

        // how long a fish waits once it reaches the current direction, before choosing a new direction
        public float maxWait = 1f;
        public float minWait = 0f;
    }
}

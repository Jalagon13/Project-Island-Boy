using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class CrabEntity : Entity
    {
        public readonly int HashIdle = Animator.StringToHash("[Anm] CrabIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] CrabMove");
        public readonly int HashChase = Animator.StringToHash("[Anm] CrabChase");
        public Action OnMove;
        public IAstarAI AI;
        [HideInInspector]
        public Tilemap IslandTilemap;

        protected override void Awake()
        {
            base.Awake();
            AI = GetComponent<IAstarAI>();
            IslandTilemap = GameObject.Find("Island").GetComponent<Tilemap>();
        }

        protected override void Update()
        {
            base.Update();
            OnMove?.Invoke();
        }

        public void Seek(Vector2 pos)
        {
            AI.destination = pos;
        }
    }
}

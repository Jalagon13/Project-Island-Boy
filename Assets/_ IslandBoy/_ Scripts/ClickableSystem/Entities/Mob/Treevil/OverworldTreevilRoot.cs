using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class OverworldTreevilRoot : MonoBehaviour
    {
        private Animator _anim;
        private AnimationClip _animClip;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _animClip = _anim.runtimeAnimatorController.animationClips[0];

            // randomize direction of root
            int rand = Random.Range(0, 2);
            if (rand == 1)
                transform.rotation = new Quaternion(0f, 180f, 0f, 1f);
        }

        void OnEnable()
        {
            // Set the time of the animation to the random time so each root is on a different frame
            _anim.Play(_animClip.name, 0, Random.Range(0f, 1f));
        }
    }
}

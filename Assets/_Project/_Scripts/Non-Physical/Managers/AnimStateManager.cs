using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public static class AnimStateManager
    {
        public static string CurrentState;
        public static int CurrentStateHash;
        public static int CurrentAnimatorInstanceID;

        public static void ChangeAnimationState(Animator animator, int newHashState)
        {
            if (CurrentStateHash == newHashState)
            {
                if (animator.GetInstanceID() != CurrentAnimatorInstanceID)
                {
                    PlayHashAnimation(animator, newHashState);
                }

                return;
            }

            PlayHashAnimation(animator, newHashState);
        }

        private static void PlayHashAnimation(Animator animator, int newHashState)
        {
            animator.Play(newHashState);
            CurrentStateHash = newHashState;
            CurrentAnimatorInstanceID = animator.GetInstanceID();
        }

        public static void ChangeAnimationState(Animator animator, AnimationClip newState)
        {
            if (CurrentState == newState.name)
            {
                if (animator.GetInstanceID() != CurrentAnimatorInstanceID)
                {
                    PlayAnimation(animator, newState);
                }

                return;
            }

            PlayAnimation(animator, newState);
        }

        private static void PlayAnimation(Animator animator, AnimationClip newState)
        {
            animator.Play(newState.name);
            CurrentState = newState.name;
            CurrentAnimatorInstanceID = animator.GetInstanceID();
        }

        // refactor later down the line
        //public static Animator ChangeAnimationState(Animator animator, int newHashState)
        //{
        //    if (CurrentStateHash == newHashState) return null;

        //    animator.Play(newHashState);
        //    CurrentStateHash = newHashState;

        //    return animator;
        //}
    }
}

using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class ThrowSpell : Throwable
	{
        [SerializeField] private Animator _animator;
        [SerializeField] private float _cooldownTimer;

        private IEnumerator Start()
		{
            yield return new WaitForSeconds(_detonationTimer);
			PlayDestroyFeedbacks();

            gameObject.GetComponent<Rigidbody2D>().simulated = false;
			_animator.enabled = true;

            DamageEntities();

			_sizzlingSource.Stop();

            yield return new WaitForSeconds(_cooldownTimer);

            Destroy(gameObject);
		}
	}
}

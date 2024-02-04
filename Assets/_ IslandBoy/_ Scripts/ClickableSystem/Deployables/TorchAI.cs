using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEditor;
using UnityEngine;

namespace IslandBoy
{
	public class TorchAI : MonoBehaviour
	{
		[SerializeField] private float _radius;
		[SerializeField] private int _initialDamage;
		[SerializeField] private float _torchDuration;
		[SerializeField] private MMF_Player _torchPlaceFeedback;
		
		private void Awake() 
		{
			StartCoroutine(DamageInRadiusDelay(_initialDamage, 0.025f));
		}
		
		private void Start() 
		{
			_torchPlaceFeedback?.PlayFeedbacks();
			
			StartCoroutine(TorchDuration());
			StartCoroutine(Burn());
		}
		
		private IEnumerator Burn()
		{
			yield return new WaitForSeconds(0.5f);
			
			StartCoroutine(DamageInRadiusDelay(1, 0.05f, false));
			
			StartCoroutine(Burn());
		}
		
		private IEnumerator TorchDuration()
		{
			yield return new WaitForSeconds(_torchDuration);
			
			StopAllCoroutines();
			Destroy(gameObject);
		}
		
		private IEnumerator DamageInRadiusDelay(int damage, float delay, bool enableKnockBack = true)
		{
			RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _radius, default);
			
			foreach (RaycastHit2D hit in hits)
			{
				if(hit.collider != null)
				{
					Entity entity = hit.collider.GetComponent<Entity>();
				
					if(entity != null)
					{
						// Damage enemies here
						entity.Damage(ToolType.Sword, damage, kbEnabled:enableKnockBack);
						yield return new WaitForSeconds(delay);
					}
				}
			}
		}
	}
}

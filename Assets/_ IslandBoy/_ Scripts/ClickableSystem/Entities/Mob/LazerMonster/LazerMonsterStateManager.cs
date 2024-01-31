using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class LazerMonsterStateManager : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private GameObject _laser;
		[SerializeField] private float _delayFire;
		
		private IEnumerator Start() 
		{
			yield return new WaitForSeconds(_delayFire);
			
			FireLaser();
			
			while(!_laser.gameObject.activeInHierarchy)
			{
				yield return new WaitForEndOfFrame();
			}
			
			StartCoroutine(Start());
		}
		
		private void FireLaser()
		{
			_laser.gameObject.SetActive(true);
		}
	}
}

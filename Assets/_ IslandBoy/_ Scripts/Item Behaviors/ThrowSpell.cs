using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class ThrowSpell : MonoBehaviour
	{
		[SerializeField] private MMF_Player _detonationFeedbacks;
		[SerializeField] private TilemapObject _floorTilemap;
		[SerializeField] private TilemapObject _wallTilemap;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _spellSource;
		[SerializeField] private float _detonationTimer;
        [SerializeField] private float _cooldownTimer;
        [SerializeField] private int _radius;
		[SerializeField] private int _breakableDamage;
		[SerializeField] private int _enemyDamage;

		private IEnumerator Start()
		{
			yield return new WaitForSeconds(_detonationTimer);
			PlayDestroyFeedbacks();

			_animator.enabled = true;

			DamageResources();
			DamageEntities();
			DamageTiles(Vector3Int.CeilToInt(transform.position), _radius);

			_spellSource.Stop();

            yield return new WaitForSeconds(_cooldownTimer);

            Destroy(gameObject);
		}
		
		private void PlayDestroyFeedbacks()
		{
			if (_detonationFeedbacks != null)
			{
				_detonationFeedbacks.transform.SetParent(null);
				_detonationFeedbacks?.PlayFeedbacks();
			}
		}
		
		public void Setup(Vector3 direction)
		{
			GetComponent<Rigidbody2D>().AddForce(direction * 30f, ForceMode2D.Impulse);
		}
		
		private void DamageTiles(Vector3Int center, int radius)
		{
			for (int x = -radius; x <= radius; x++)
			{
				for (int y = -radius; y <= radius; y++)
				{
					Vector3Int offset = new Vector3Int(x, y, 0);
					Vector3Int position = center + offset;

					// Check if the distance from center is within the specified radius
					if (Vector3Int.Distance(center, position) <= radius)
					{
						// Check if the position is within the bounds of the tilemap
						if (_wallTilemap.Tilemap.GetTile(position) != null)
						{
							_wallTilemap.DynamicTilemap.DestroyTile(position);
						}
						
						if (_floorTilemap.Tilemap.GetTile(position) != null)
						{
							_floorTilemap.DynamicTilemap.DestroyTile(position);
						}
					}
				}
			}
		}
		
		private void DamageResources()
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius);

			foreach (Collider2D collider in colliders)
			{
				Resource rsc = collider.GetComponent<Resource>();

				if (rsc != null)
				{
					if(rsc.SwingDestructOnly)
					{
						var gmod = rsc.GetComponent<GiveMoneyOnDestroy>();
						
						if(gmod != null)
						{
							gmod.GiveMoney();
						}
						
						rsc.OnBreak();
					}

					rsc.OnHit(ToolType.Pickaxe, _breakableDamage);
					rsc.OnHit(ToolType.Axe, _breakableDamage);
					rsc.OnHit(ToolType.Hammer, _breakableDamage);
				}
			}
		}

		private void DamageEntities()
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius);

			foreach (Collider2D collider in colliders)
			{
				Entity entity = collider.GetComponent<Entity>();
				
				if(entity != null)
				{
					entity.Damage(ToolType.Sword, _enemyDamage);
				}
				
				Player player = collider.GetComponent<Player>();
				
				if(player != null)
				{
					player.Damage(_enemyDamage, transform.position);
				}
			}
		}
	}
}

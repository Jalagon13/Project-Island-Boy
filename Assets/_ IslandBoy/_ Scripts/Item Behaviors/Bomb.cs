using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class Bomb : MonoBehaviour
	{
		[SerializeField] private TilemapObject _floorTilemap;
		[SerializeField] private TilemapObject _wallTilemap;
		[SerializeField] private AudioClip _explosionSound;
		[SerializeField] private AudioSource _sizzlingSource;
		[SerializeField] private float _detonationTimer;
		[SerializeField] private int _explodeRadius;
		[SerializeField] private int _breakableDamage;
		[SerializeField] private int _enemyDamage;

		private IEnumerator Start()
		{
			yield return new WaitForSeconds(_detonationTimer);

			BlowUpResources();
			BlowUpEntities();
			BlowUpTiles(Vector3Int.CeilToInt(transform.position), _explodeRadius);

			_sizzlingSource.Stop();

			MMSoundManagerSoundPlayEvent.Trigger(_explosionSound, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);

			Destroy(gameObject);
		}
		
		public void Setup(Vector3 direction)
		{
			GetComponent<Rigidbody2D>().AddForce(direction * 30f, ForceMode2D.Impulse);
		}
		
		private void BlowUpTiles(Vector3Int center, int radius)
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
		
		private void BlowUpResources()
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explodeRadius);

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

		private void BlowUpEntities()
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explodeRadius);

			foreach (Collider2D collider in colliders)
			{
				Entity entity = collider.GetComponent<Entity>();
				
				if(entity != null)
				{
					entity.OnHit(ToolType.Sword, _enemyDamage);
				}
			}
		}
	}
}

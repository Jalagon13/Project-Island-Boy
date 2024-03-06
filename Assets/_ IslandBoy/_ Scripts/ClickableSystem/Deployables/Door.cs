using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
	public class Door : Interactable
	{
		[Header("Base Door Parameters")]
		[SerializeField] private Sprite _openSprite;
		[SerializeField] private Sprite _closeSprite;
		[SerializeField] private AudioClip _doorOpenSound;
		[SerializeField] private AudioClip _doorCloseSound;
		[SerializeField] private TilemapObject _groundTm;


		private Collider2D _doorCollider;
		private bool _opened;

		protected override void Awake()
		{
			base.Awake();
			_doorCollider = transform.GetChild(1).GetComponent<Collider2D>();
			_sr = transform.GetChild(0).GetComponent<SpriteRenderer>();

		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			var playerFeet = other.GetComponent<FeetTag>();
			
			if(playerFeet != null || other.CompareTag("NPC"))
			{
				Open();
			}
		}
		
		protected override void OnTriggerExit2D(Collider2D other)
		{
			base.OnTriggerExit2D(other);
			
			var playerFeet = other.GetComponent<FeetTag>();
			
			if(playerFeet != null || other.CompareTag("NPC"))
			{
				Close();
			}
		}

		public override void Interact()
		{
			_instructions.gameObject.SetActive(false);

			if (!_canInteract) return;
			if (_opened)
				Close();
			else
				Open();
		}

		public void Open()
		{
			_doorCollider.gameObject.SetActive(false);
			_opened = true;
			_sr.sprite = _openSprite;

			MMSoundManagerSoundPlayEvent.Trigger(_doorOpenSound, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
			// _groundTm.DynamicTilemap.UpdateNavMesh();
		}

		public void Close()
		{
			_doorCollider.gameObject?.SetActive(true);
			_opened = false;
			_sr.sprite = _closeSprite;

			MMSoundManagerSoundPlayEvent.Trigger(_doorCloseSound, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
			// _groundTm.DynamicTilemap.UpdateNavMesh();
		}

		public override void ShowDisplay()
		{
			_instructions.gameObject.SetActive(true);
		}
	}
}

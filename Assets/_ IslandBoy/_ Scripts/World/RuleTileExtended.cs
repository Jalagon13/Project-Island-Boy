using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	[CreateAssetMenu(menuName = "New Extended Rule Tile")]
	public class RuleTileExtended : RuleTile
	{
		[Header("Rule Tile Extensions")]
		public ItemObject Item;
		public AudioClip HitSound;
		public AudioClip PlaceSound;
		public AudioClip DestroySound;
		public int MaxHitPoints;
		public ToolType HitToolType;
	}
}

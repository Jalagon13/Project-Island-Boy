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
		
		public void UpdatePathfinding(Vector2 pos)
		{
			Bounds updateBounds = new(pos, new(2, 2, 1));
			// AstarPath.active.UpdateGraphs(updateBounds, 0.1f);
		}
	}
}

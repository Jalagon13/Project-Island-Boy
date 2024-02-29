using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Pathfinding;
using UnityEngine;

namespace IslandBoy
{
	public class Laser : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private AudioClip _laserSound;
		[SerializeField] private float _chargeDuration;
		[SerializeField] private float _waitBeforeFireDuration;
		[SerializeField] private float _laserDuration;
		[SerializeField] private float laserWidth;
		
		private LineRenderer _line;
		private PolygonCollider2D _damageCollider;
		private List<Vector2> _colliderPoints = new();
		
		private void Awake() 
		{
			_line = GetComponent<LineRenderer>();
			_damageCollider = GetComponent<PolygonCollider2D>();
		}
		
		private void OnEnable() 
		{
			StartCoroutine(FireBeam());
		}
		
		private IEnumerator FireBeam()
		{
			UpdateLineWidth(0.1f, 0.1f, new Color(1, 0, 0, .4f));
			_damageCollider.enabled = false;
			
			float counter = 0;
			while(counter < _chargeDuration)
			{
				counter += Time.deltaTime;
				UpdateAimRenderer();
				yield return new WaitForEndOfFrame();
			}
			
			// freeze the aim for a few seconds
			yield return new WaitForSeconds(_waitBeforeFireDuration);
			
			// fire
			yield return StartCoroutine(Fire());
		}
		
		private IEnumerator Fire()
		{
			yield return new WaitForEndOfFrame();
			UpdateLineWidth(laserWidth, laserWidth, new Color(1, 0, 0, 1f));
			BuildDamageMesh(); 
			
			MMSoundManagerSoundPlayEvent.Trigger(_laserSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
			
			yield return new WaitForSeconds(_laserDuration);
			_damageCollider.enabled = false;
			gameObject.SetActive(false);
		}
		
		private void BuildDamageMesh()
		{
			_colliderPoints = CalculateColliderPoints();
			_damageCollider.SetPath(0, _colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
			_damageCollider.enabled = true;
		}
		
		private List<Vector2> CalculateColliderPoints()
		{
			// Get ALL positions on the line renderer
			Vector3[] positions = GetPositions();
			
			// Get the Width of the line
			float width = GetWidth();
			
			// m = (y2 - y1) / (x2 - x1)
			float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
			float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
			float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));
			
			// Calculate the Offset from each point to the collision vertex
			Vector3[] offsets = new Vector3[2];
			offsets[0] = new Vector3(-deltaX, deltaY);
			offsets[1] = new Vector3(deltaX, -deltaY);
			
			// Generate the Colliders Vertices
			List<Vector2> colliderPositions = new List<Vector2>
			{
				positions[0] + offsets[0],
				positions[1] + offsets[0],
				positions[1] + offsets[1],
				positions[0] + offsets[1],
			};
			
			return colliderPositions;
		}
		
		private Vector3[] GetPositions()
		{
			Vector3[] positions = new Vector3[_line.positionCount];
			_line.GetPositions(positions);
			return positions;
		}
		
		private float GetWidth()
		{
			return _line.startWidth;
		}
		
		private void UpdateLineWidth(float start, float end, Color lineColor)
		{
			_line.startWidth = start;
			_line.endWidth = end;
			_line.startColor = lineColor;
			_line.endColor = lineColor;
		}
		
		private void UpdateAimRenderer()
		{
			Vector2 origin = transform.parent.transform.position;
			Vector2 target = _po.Position + Vector2.up * 0.5f;
			Vector2 direction = (target - origin).normalized;
			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, Mathf.Infinity);
			
			foreach (RaycastHit2D item in hits)
			{
				if(item.collider.gameObject.layer == 8)
				{
					_line.SetPosition(0, origin);
					_line.SetPosition(1, item.point);
					return;
				}
			}
		}
	}
}

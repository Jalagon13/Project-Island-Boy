using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
	public class Bed : MonoBehaviour
	{
		[SerializeField] private TilemapObject _wallTm;
		[SerializeField] private TilemapObject _floorTm;
		[SerializeField] private TextMeshProUGUI _bedText;
		[SerializeField] private GameObject _sleepWarning;

		[SerializeField] private GameObject _npcIcons;
		private BedNpcButton _npcButton;

		private bool _canCheck;
		private bool _occupied;
		private Resident _resident;
		private List<Vector3Int> _floorTilePositions = new();

		public bool Occupied { get { return _occupied; } }
		public Resident Resident { get { return _resident; } }


		public void Awake()
		{
			GameSignals.NPC_FREED.AddListener(UpdateNpcIcons);
		}

		public void Start()
		{
			_canCheck = true;
			UpdateNpcIcons();
		}

		public void OnDestroy()
		{
			MoveOutNPC();
			GameSignals.NPC_FREED.RemoveListener(UpdateNpcIcons);
		}

		public void TryToEndDay() // connected to bed button
		{
			if (!_canCheck) return;

			// DispatchEvents();
			
			if (InValidSpace())
			{
				Player.RESTED_STATUS = RestedStatus.Good;
				//_bedText.text = "<color=green>Valid House Detected:</color=green> <br><br>This bed is in a valid house, sleep penality will be avoided.";
				Sleep();
			}
			else
			{
				Player.RESTED_STATUS = RestedStatus.Okay;
				//_bedText.text = "<color=red>Sleep Penality Detected!:</color=red> <br><br>Sleeping in a bed in an uncomplete house will apply a sleep penality upon the next day.";
				_sleepWarning.SetActive(true);
			}
		}
		
		public void Sleep() // attached to sleep button
		{
			DispatchEvents();
		}

		public void MoveInNPC(Resident resident)
		{
			_resident = resident;
			_occupied = true;
			_resident.SetBed(this);
			_npcButton = GetNPCButton();
			_npcButton.Select();
			ResetNpc();
		}
		
		public void MoveOutNPC()
		{
			if(_resident != null)
			{
				_resident.RemoveBed();
				_resident = null;
				_occupied = false;
				_npcButton.Deselect();
				_npcButton = null;
			}
		}
		
		public void ResetNpc()
		{
			if(_occupied)
			{
				_resident.SetPosition(_floorTilePositions[Random.Range(0, _floorTilePositions.Count)] + new Vector3(0.5f, 0.5f));
			}
		}

		public void ReassignNpc(string npc)
		{
			if (!(_resident != null && _resident.Name == npc))
			{
				MoveOutNPC();
				if (InValidSpace())
				{
					Resident res = HousingController.Instance.GetResident(npc);
					if (res != null)
					{
						if (res.Bed != null)
							res.Bed.MoveOutNPC();
						MoveInNPC(res);
					}
				}
			}
			else
			{
				MoveOutNPC();
			}
		}

		private BedNpcButton GetNPCButton()
		{
			return _npcIcons.transform.Find(_resident.Name).GetComponent<BedNpcButton>();
		}

		private void UpdateNpcIcons(ISignalParameters parameters)
		{
			UpdateNpcIcons();
		}

		private void UpdateNpcIcons()
		{
			foreach (string npc in NpcSlots.Instance.AllNPCs)
			{
				if (NpcSlots.Instance.HasBeenFreed(npc))
				{
					_npcIcons.transform.Find(npc).gameObject.SetActive(true);
				}
			}
		}

		// private Vector3 ReturnRandomFloorTile()
		// {
		// 	var randomIndex = Random.Range(0, _floorTilePositions.Count - 1);

		// 	return _floorTilePositions[randomIndex];
		// }

		private void DispatchEvents()
		{
			GameSignals.BED_TIME_EXECUTED.Dispatch();
			GameSignals.DAY_END.Dispatch();
		}
		
		public bool InValidSpace() // check for floors and walls for house is valid. check furniture too. make floortilePos a global var.
		{
			Stack<Vector3Int> tilesToCheck = new();
			_floorTilePositions = new();
			List<Vector3Int> wallTilePositions = new(); // list of wall tiles around the free space
			List<Vector3Int> doorPositions = new(); // list of all positions of doors if any door is found

			var checkPos = Vector3Int.FloorToInt(transform.root.position);
			tilesToCheck.Push(checkPos);

			while (tilesToCheck.Count > 0)
			{
				var p = tilesToCheck.Pop();

				// if there is a tile without floor or wall then it is not an enclosed area
				if (!_floorTm.Tilemap.HasTile(p) && !_wallTm.Tilemap.HasTile(p) && !HasDoor(p))
				{
					//_feedbackHolder.DisplayFeedback("Not valid housing. Make sure the area around you is enclosed.", Color.yellow);
					// PopupMessage.Create(transform.position, "Area not enclosed with floor and wall or missing a door", Color.yellow, new(0.5f, 0.5f), 3f);
					Debug.Log("1");
					Debug.Log(!_floorTm.Tilemap.HasTile(p));
					Debug.Log(!_wallTm.Tilemap.HasTile(p));
					Debug.Log(!HasDoor(p));
					Debug.Log(p);
					return false;
				}

				// if there is a door, add it do the door positions
				if (HasDoor(p))
				{
					doorPositions.Add(p);
					continue;
				}

				// if there is a bed in the house other than this bed, not valid housing
				if (HasBed(p))
				{
					print("Too many beds!");
					Debug.Log("2");
					return false;
				}

				// if tile has a wall, continue
				if (_wallTm.Tilemap.HasTile(p))
				{
					wallTilePositions.Add(p);
					continue;
				}

				// add floor tile to floorTilePositions and push new tiles to check
				if (!_floorTilePositions.Contains(p))
				{
					_floorTilePositions.Add(p);

					tilesToCheck.Push(new Vector3Int(p.x - 1, p.y));
					tilesToCheck.Push(new Vector3Int(p.x + 1, p.y));
					tilesToCheck.Push(new Vector3Int(p.x, p.y - 1));
					tilesToCheck.Push(new Vector3Int(p.x, p.y + 1));
				}
			}

			// if floor tile positions are less than 9 tiles, then housing is too small.
			if (_floorTilePositions.Count < 9)
			{
				// PopupMessage.Create(transform.position, "Space too small!", Color.yellow, new(0.5f, 0.5f), 1f);
				Debug.Log("3");
				return false;
			}

			// if floor tile positions are greater than 100 tiles, then housing is too big.
			if (_floorTilePositions.Count > 200)
			{
				// PopupMessage.Create(transform.position, "Space too large!", Color.yellow, new(0.5f, 0.5f), 1f);
				Debug.Log("4");
				return false;
			}

			// if there is no doors found then housing not valid
			if (doorPositions.Count <= 0)
			{
				// PopupMessage.Create(transform.position, "No door found!", Color.yellow, new(0.5f, 0.5f), 1f);
				Debug.Log("5");
				return false;
			}

			// loop through all doors found
			// a valid door is if the door is flanked n/s or e/w by wall tiles and check if one
			// side of the empty space door is part of the floor tile positions and one side is not.
			bool validDoorFound = false;

			foreach (Vector3Int p in doorPositions)
			{
				Vector3Int nn = new(p.x, p.y + 1);
				Vector3Int sn = new(p.x, p.y - 1);
				Vector3Int en = new(p.x + 1, p.y);
				Vector3Int wn = new(p.x - 1, p.y);

				if (_wallTm.Tilemap.HasTile(nn) && _wallTm.Tilemap.HasTile(sn))
				{
					int counter = 0;

					if (_floorTilePositions.Contains(en))
						counter++;

					if (_floorTilePositions.Contains(wn))
						counter++;

					if (counter == 1)
					{
						validDoorFound = true;
						break;
					}
				}

				if (_wallTm.Tilemap.HasTile(en) && _wallTm.Tilemap.HasTile(wn))
				{
					int counter = 0;

					if (_floorTilePositions.Contains(nn))
						counter++;

					if (_floorTilePositions.Contains(sn))
						counter++;

					if (counter == 1)
					{
						validDoorFound = true;
						break;
					}
				}
			}

			if (!validDoorFound)
			{
				// PopupMessage.Create(transform.position, "Door must lead to outside!", Color.yellow, new(0.5f, 0.5f), 1f);
				Debug.Log("6");
				return false;
			}

			return true;
		}

		private bool HasDoor(Vector3Int pos)
		{
			var centerPos = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
			var colliders = Physics2D.OverlapCircleAll(centerPos, 0.1f);

			foreach (Collider2D col in colliders)
			{
				if (col.TryGetComponent(out Door door))
					return true;
			}

			return false;
		}

		private bool HasBed(Vector3Int pos)
		{
			var centerPos = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
			var colliders = Physics2D.OverlapCircleAll(centerPos, 0.1f);

			foreach (Collider2D col in colliders)
			{
				if (col.TryGetComponent(out Bed bed))
				{
					if (transform.position != col.transform.position)
						return true;
				}
			}

			return false;
		}
	}
}

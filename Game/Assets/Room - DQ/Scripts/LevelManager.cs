using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class LevelManager : MonoBehaviour {

	private GameObject[] rooms;
//	private Stopwatch mission_time;
//	private bool game_over = false;

	// Use this for initialization
	void Start () {


		// 1. Split rooms
		// 2. Randomly rotate rooms before making ship accessible
		// 3. Rotate rooms until fully accessible ship
		// 4. Delete doors/walls appropriately
		// 5. Remove overlapping walls
		// 6. Dynamic floor

		SplitRooms ();
		rooms = GameObject.FindGameObjectsWithTag("Room");
		RandomRoomRotation ();
		MakeShipAccessible ();
		RemoveWallDoors ();
		RemoveOverlappingWalls ();
		CalculateFloor ();
		GenerateNPCs ();

		GameObject.FindGameObjectWithTag ("MissionManager").GetComponent<MissionManager> ().LevelLoaded ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	private bool CheckShipAccessible()
	{
		bool ret = false;
		foreach(GameObject room in rooms)
		{
			if(!room.GetComponent<RoomManager>().safe)
			{
				ret = true;
				break;
			}
			ret = false;
		}
		return ret;
	}

	private void CalculateFloor()
	{
		GameObject[] hallways = GameObject.FindGameObjectsWithTag("DynamicFloor");
		foreach(GameObject hallway in hallways)
		{
			hallway.GetComponent<DynamicFloor>().Calculate();
		}
	}

	private void RemoveWallDoors()
	{
		GameObject[] door_spawns = GameObject.FindGameObjectsWithTag("Door Spawn");
		foreach(GameObject door_spawn in door_spawns)
		{
			//get door
			Transform[] transforms = door_spawn.GetComponentsInChildren<Transform>();
			List<GameObject> walls = new List<GameObject>();
			bool delete_wall = false;
			foreach(Transform transform in transforms)
			{
				if(transform.CompareTag("Door"))
				{
					if(transform.gameObject.GetComponent<DoorManager>().GetAccess())
					{
						delete_wall = true;
					}
					else
					{
						Destroy(transform.gameObject);
						break;
					}
				}
				else if(transform.CompareTag("Wall"))
				{
					walls.Add(transform.gameObject);
				}
			}
			if(delete_wall)
			{
				foreach(GameObject wall in walls)
				{
					Destroy(wall);
				}
			}
		}
	}

	private void MakeShipAccessible ()
	{
		foreach(GameObject room in rooms)
		{
			room.GetComponent<RoomManager>().SafeNeighbourCheck();
		}
		bool running = false;
		do {
			//rotate inaccessible rooms
			foreach(GameObject room in rooms)
			{
				if(!room.GetComponent<RoomManager>().safe)
				{
					room.GetComponent<RoomManager>().Rotate();
					room.GetComponent<RoomManager>().SafeNeighbourCheck();
				}
			}
			
			//check if ship is accessible
			running = CheckShipAccessible();
		} while (running);
		
		foreach(GameObject r in rooms)
		{
			r.GetComponent<RoomManager>().SafeNeighbourCheck();
		}
	}

	private void SplitRooms()
	{
		for(int i = 0; i < 3; i++)
		{
			GameObject[] room_templates = GameObject.FindGameObjectsWithTag("RoomTemplate");
			foreach(GameObject template in room_templates)
			{
				template.GetComponent<RoomSplitter>().Split();
			}
		}
	}

	private void RandomRoomRotation()
	{
		foreach(GameObject room in rooms)
		{
			int r = Random.Range(0, 4);
			for(int i = 0; i < r; i++)
			{
				room.GetComponent<RoomManager>().Rotate();
			}
		}
	}

	private void RemoveOverlappingWalls ()
	{
//		GameObject[] all_walls = GameObject.FindGameObjectsWithTag("Wall");
//		for(int i = 0; i < all_walls.Length; i++)
//		{
//			for(int j = 0; j < all_walls.Length; j++)
//			{
//				if(all_walls[j] == null || all_walls[i] == null)
//					continue;
//				if(all_walls[i] != all_walls[j] && Vector3.Distance(all_walls[i].transform.position, all_walls[j].transform.position) < 0.25f)
//				{
//					Destroy(all_walls[j]);
//				}
//			}
//		}
	}

	private void GenerateNPCs ()
	{
		MissionType mt = GameObject.FindGameObjectWithTag ("MissionManager").GetComponent<MissionManager> ().ActiveMissionType ();
		if (mt == MissionType.NUM_OF_MISSIONS)
			return;

		foreach (GameObject room in rooms)
		{
			room.GetComponent<NPCGenarator>().Initialise(mt);
		}
	}
}

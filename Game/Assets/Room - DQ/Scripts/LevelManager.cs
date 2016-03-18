using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	private GameObject[] room_templates, rooms, door_spawns, hallways;

	// Use this for initialization
	void Start () {
		//split rooms
		for(int i = 0; i < 3; i++)
		{
			room_templates = GameObject.FindGameObjectsWithTag("RoomTemplate");
			foreach(GameObject template in room_templates)
			{
				template.GetComponent<RoomSplitter>().Split();
			}
		}

		//rotate rooms until fully accessible ship
		rooms = GameObject.FindGameObjectsWithTag("Room");
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

		//delete doors/walls appropriately
		door_spawns = GameObject.FindGameObjectsWithTag("Door Spawn");
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

		//remove overlapping walls
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

		//dynamic floor
		hallways = GameObject.FindGameObjectsWithTag("DynamicFloor");
		//Debug.Log(hallways.Length);
		foreach(GameObject hallway in hallways)
		{
			hallway.GetComponent<DynamicFloor>().Calculate();
		}
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
}

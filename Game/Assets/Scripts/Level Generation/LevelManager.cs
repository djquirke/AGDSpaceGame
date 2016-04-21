using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class LevelManager : MonoBehaviour {

	private GameObject[] rooms;
	private MissionType mt;
	private Difficulty difficulty;
	private int MAX_NPC_COUNT = 25;

    private Object ActiveLoadingScreen = null;

    private bool Loading = false;
	private bool nodes_disabled = false;
	private bool npcs_generated = false;
	private bool events_randomised = false;

	// Use this for initialization
	void Start () {
		
		mt = GameObject.FindGameObjectWithTag ("MissionManager").GetComponent<MissionManager> ().ActiveMissionType ();
		difficulty = GameObject.FindGameObjectWithTag ("MissionManager").GetComponent<MissionManager> ().ActiveMissionDifficulty();

        Loading = true;

        ActiveLoadingScreen = Instantiate(GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().LoadingScreen);

        StartCoroutine("GenerateLevel");
	}
	
	// Update is called once per frame
	void Update () {

        if(ActiveLoadingScreen && !Loading)
        {
            Destroy(ActiveLoadingScreen);
            ActiveLoadingScreen = null;
        }
	}


    private void GenerateLevel()
    {
        // 1. Split rooms
        // 2. Randomly rotate rooms before making ship accessible
        // 3. Rotate rooms until fully accessible ship
        // 4. Delete doors/walls appropriately
        // 5. Remove overlapping walls
        // 6. Dynamic floor
		//yield return new WaitForEndOfFrame();
        UnityEngine.Debug.Log("splitting rooms");
        SplitRooms();
        rooms = GameObject.FindGameObjectsWithTag("Room");
        UnityEngine.Debug.Log("random rotating rooms");
        RandomRoomRotation();
        UnityEngine.Debug.Log("making ship accessible");
        MakeShipAccessible();
        UnityEngine.Debug.Log("removing walls and doors");
        RemoveWallDoors();
        UnityEngine.Debug.Log("rm overlapping walls");
        //RemoveOverlappingWalls();
        UnityEngine.Debug.Log("calc floor");
        CalculateFloor();
        UnityEngine.Debug.Log("check disable engineering");
        CheckDisableEngineering();
        CheckDisableOxygen();
		StartCoroutine(DisableInvalidNodes());
        UnityEngine.Debug.Log("generate npcs");
		StartCoroutine(GenerateNPCs ());
		UnityEngine.Debug.Log("spawning player");
        SpawnPlayer();
        UnityEngine.Debug.Log("randomising events");
		StartCoroutine(RandomiseEvents());
		StartCoroutine(GenerateAIPaths());

        
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
					DestroyImmediate(wall);
				}
			}
		}
	}

	private void MakeShipAccessible ()
	{
//		foreach(GameObject room in rooms)
//		{
//			room.GetComponent<RoomManager>().SafeNeighbourCheck();
//		}
		bool running = false;
		do {
			//rotate inaccessible rooms
			foreach(GameObject room in rooms)
			{
				RoomManager rm = room.GetComponent<RoomManager>();
				rm.SafeNeighbourCheck();
				if(!rm.safe)
				{
					rm.Rotate();
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
			GameObject[] dmbs = GameObject.FindGameObjectsWithTag("DMB");

			foreach(GameObject dmb in dmbs)
			{
				int r = Random.Range(0, 2);
				if(r == 0)
					dmb.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
			}

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
			if(room.GetComponent<RoomManager>().type == RoomType.FLIGHT_DECK) continue;

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

	private IEnumerator GenerateNPCs ()
	{
		while(!nodes_disabled)
			yield return new WaitForSeconds(0.5f);
		if (mt == MissionType.NUM_OF_MISSIONS)
			yield break;

		foreach (GameObject room in rooms)
		{
			room.GetComponent<NPCGenarator>().Initialise(mt);
		}

		GameObject[] npcs = GameObject.FindGameObjectsWithTag("AI");
		UnityEngine.Debug.Log("num of npcs:" + npcs.Length);
		if(npcs.Length > MAX_NPC_COUNT)
		{
			List<int> the_chosen_ones = ChooseRandomNumbers(MAX_NPC_COUNT, npcs.Length);
			UnityEngine.Debug.Log("number of npcs to keep:" + the_chosen_ones.Count);
			for(int i = 0; i < npcs.Length; i++)
			{
				if(the_chosen_ones.Contains(i))
				{
					continue;
				}
				else
				{
					UnityEngine.Debug.Log("destroying npc:" + i);
					Destroy(npcs[i]);
				}
			}
		}
		npcs_generated = true;
	}

	private void SpawnPlayer()
	{
		GameObject.FindGameObjectWithTag("PlayerSpawn").GetComponent<SpawnCharacter>().SpawnPlayer(mt);
	}

	private int CountMinigames()
	{
		GameObject[] events = GameObject.FindGameObjectsWithTag("Event");
		return events.Length;
	}

	//TODO: remove all broken engineer panels or fixed ones based on if it is an engineer level
	private void CheckDisableEngineering()
	{
		UnityEngine.Debug.Log("Level manager mission type:" + mt);
		if(mt == MissionType.ENGINEERING) return;
		else
		{
			foreach(GameObject room in rooms)
			{
				if(room.GetComponent<RoomManager>().type == RoomType.ENGINEER)
				{
					Transform[] tforms = room.GetComponentsInChildren<Transform>();
					foreach(Transform tform in tforms)
					{
						if(tform.tag.Equals("Event"))
						{
							tform.tag = "Untagged";
						}
					}
				}
			}
		}

	}

	private void CheckDisableOxygen()
	{
		if(mt == MissionType.OXYGEN) return;
		else
		{
			foreach(GameObject room in rooms)
			{
				if(room.GetComponent<RoomManager>().type == RoomType.MEDIC)
				{
					Transform[] tforms = room.GetComponentsInChildren<Transform>();
					foreach(Transform tform in tforms)
					{
						if(tform.tag.Equals("Event"))
						{
							tform.tag = "Untagged";
						}
					}
				}
			}
		}

	}

	private IEnumerator RandomiseEvents ()
	{
		while(!npcs_generated)
			yield return new WaitForSeconds(0.1f);

		int x = CountMinigames();
		UnityEngine.Debug.Log ("Minigames to choose from:" + x);

		List<int> the_chosen_ones = new List<int> ();
		int r;
		switch (difficulty) {
		case Difficulty.Easy:
			r = Random.Range(2, 4);
			the_chosen_ones = ChooseRandomNumbers(r, x);
			break;
		case Difficulty.Medium:
			r = Random.Range(3, 5);
			the_chosen_ones = ChooseRandomNumbers(r, x);
			break;
		case Difficulty.Hard:
			r = Random.Range(3, 5);
			the_chosen_ones = ChooseRandomNumbers(r, x);
			break;
		case Difficulty.Insane:
			r = Random.Range(3, 6);
			the_chosen_ones = ChooseRandomNumbers(r, x);
			break;
		default:
			break;
		}

		GameObject[] events = GameObject.FindGameObjectsWithTag("Event");

		for (int i = 0; i < x; i++)
		{
			if(the_chosen_ones.Contains(i))
			{
				//events[i].GetComponent<Event>().Initialise();
				continue;
			}

			if(mt == MissionType.ILLNESS)
				Destroy(events[i]);
			events[i].GetComponent<Event>().EventNotNeeded();
		}

		events_randomised = true;
	}

	private List<int> ChooseRandomNumbers(int num, int events)
	{
		List<int> temp = new List<int> ();
        if(events < num)
        {
            UnityEngine.Debug.LogError("Number of events wanted " + num + " number of events found " + events);
            return temp;
        }

		int counter = 0;
		while (counter < num)
		{
			int r = Random.Range(0, events);
			if(temp.Contains(r)) continue;
			temp.Add(r);
			counter++;
		}

		return temp;
	}

	private IEnumerator GenerateAIPaths()
	{
		while(!events_randomised)
			yield return new WaitForSeconds(0.5f);
		GameObject[] agents = GameObject.FindGameObjectsWithTag("AI");
		UnityEngine.Debug.Log("num of agents:" + agents.Length);
		foreach(GameObject agent in agents)
		{
			agent.GetComponent<AIMovement>().Initialise();
		}

		int minigames = CountMinigames();
		UnityEngine.Debug.Log("level loaded");
		GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().LevelLoaded(minigames);
		UnityEngine.Debug.Log("done");
		Loading = false;
	}
	
	private IEnumerator DisableInvalidNodes()
	{
		yield return new WaitForEndOfFrame();
		GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
		List<Transform> destroyed_nodes = new List<Transform>();
		foreach(GameObject node in nodes)
		{
			Collider[] collisions = Physics.OverlapSphere(node.transform.position, 0.05f);
			int node_counter = 0;
			foreach(Collider col in collisions)
			{
				if(col.CompareTag("Wall") || col.CompareTag("Block"))
				{
					Destroy(node);
				}
				else if(col.CompareTag("Node"))
				{
					node_counter++;
					if(node_counter > 1)
					{
						bool destroy = true;
						foreach(Transform t in destroyed_nodes)
						{
							if(Vector3.Distance(t.position, col.transform.position) < 0.1f)
							{
								destroy = false;
							}
						}
						if(destroy)
						{
							destroyed_nodes.Add(node.transform);
							Destroy (node);
						}
					}
				}
			}
		}
		nodes_disabled = true;
	}
}
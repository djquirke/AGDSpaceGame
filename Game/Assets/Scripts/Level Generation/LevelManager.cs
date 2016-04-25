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
	private bool rooms_split = false;
	private bool rooms_rotated = false;
	private bool ship_accessible = false;
	private bool wall_doors_removed = false;
	private bool ol_columns_removed = false;
	private bool floor_calculated = false;
	private bool engineering_disabled = false;
	private bool oxygen_disabled = false;
	private bool player_spawned = false;
	private float wait_time = 0.3f;

	private LoadingScreen ls;

	// Use this for initialization
	void Start () {
		MissionManager mm = GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>();
		mt = mm.ActiveMissionType ();
		difficulty = mm.ActiveMissionDifficulty();

        Loading = true;

        ActiveLoadingScreen = Instantiate(mm.LoadingScreen);
		//ls = mm.LoadingScreen.GetComponent<LoadingScreen>();
		//UnityEngine.Debug.Log(ls);
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


    private IEnumerator GenerateLevel()
    {
        // 1. Split rooms
        // 2. Randomly rotate rooms before making ship accessible
        // 3. Rotate rooms until fully accessible ship
        // 4. Delete doors/walls appropriately
        // 5. Remove overlapping walls
        // 6. Dynamic floor
		////yield return new WaitForEndOfFrame();
		ls = GameObject.FindGameObjectWithTag("LoadScreen").GetComponentInChildren<LoadingScreen>();
		yield return new WaitForSeconds(wait_time);
		ls.current_loading_phase = "Generating Rooms...";
		yield return new WaitForSeconds(wait_time);
		//LoadingScreen ls = mm.LoadingScreen.GetComponent<LoadingScreen>();
		UnityEngine.Debug.Log(ls);
        StartCoroutine(SplitRooms());
		rooms = GameObject.FindGameObjectsWithTag("Room");
		StartCoroutine(RandomRoomRotation());
		StartCoroutine(MakeShipAccessible());
		StartCoroutine(RemoveWallDoors());
		StartCoroutine(RemoveOverlappingColumns());
		StartCoroutine(CalculateFloor());
        StartCoroutine(CheckDisableEngineering());
		StartCoroutine(CheckDisableOxygen());
		StartCoroutine(DisableInvalidNodes());
		StartCoroutine(GenerateNPCs ());
		StartCoroutine(SpawnPlayer());
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

	private IEnumerator CalculateFloor()
	{
		while(!ol_columns_removed)
			yield return new WaitForSeconds(wait_time);
		//yield return new WaitForEndOfFrame();
		ls.current_loading_phase = "Calculating Dynamic Floor...";
		GameObject[] hallways = GameObject.FindGameObjectsWithTag("DynamicFloor");
		foreach(GameObject hallway in hallways)
		{
			hallway.GetComponent<DynamicFloor>().Calculate();
		}
		floor_calculated = true;
	}

	private IEnumerator RemoveWallDoors()
	{
		while(!ship_accessible)
			yield return new WaitForSeconds(wait_time);
		//yield return new WaitForSeconds(1f);
		ls.current_loading_phase = "Removing Specific Walls and Doors...";
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
					UnityEngine.Debug.Log("door gives access:" + transform.gameObject.GetComponent<DoorManager>().GetAccess());
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
		wall_doors_removed = true;
	}

	private IEnumerator MakeShipAccessible ()
	{
//		foreach(GameObject room in rooms)
//		{
//			room.GetComponent<RoomManager>().SafeNeighbourCheck();
//		}
		while(!rooms_rotated)
			yield return new WaitForSeconds(wait_time);
		//yield return new WaitForEndOfFrame();
		ls.current_loading_phase = "Making Ship Accessible...";
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
		//yield return new WaitForEndOfFrame();
		ship_accessible = true;
	}

	private IEnumerator SplitRooms()
	{
		//yield return new WaitForEndOfFrame();
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
		rooms_split = true;
		yield break;
	}

	private IEnumerator RandomRoomRotation()
	{
		while(!rooms_split)
			yield return new WaitForSeconds(wait_time);
		//yield return new WaitForEndOfFrame();
		ls.current_loading_phase = "Randomly Rotating Rooms...";
		foreach(GameObject room in rooms)
		{
			if(room.GetComponent<RoomManager>().type == RoomType.FLIGHT_DECK) continue;

			int r = Random.Range(0, 4);
			for(int i = 0; i < r; i++)
			{
				room.GetComponent<RoomManager>().Rotate();
			}
		}
		rooms_rotated = true;
	}

	private IEnumerator RemoveOverlappingColumns ()
	{
		while(!wall_doors_removed)
			yield return new WaitForSeconds(wait_time);
		yield return new WaitForEndOfFrame();
		ls.current_loading_phase = "Removing Overlapping Columns...";
		GameObject[] all_caps = GameObject.FindGameObjectsWithTag("Cap");
		foreach(GameObject cap in all_caps)
		{
			Collider[] cols = Physics.OverlapSphere(cap.transform.position, 0.3f);
			foreach(Collider col in cols)
			{
				if(col.CompareTag("Column"))
				{
					Destroy (cap);
				}
			}
		}
		ol_columns_removed = true;
	}

	private IEnumerator GenerateNPCs ()
	{
		while(!nodes_disabled)
			yield return new WaitForSeconds(wait_time);
		if (mt == MissionType.NUM_OF_MISSIONS)
			yield break;
		yield return new WaitForEndOfFrame();
		ls.current_loading_phase = "Generating NPC's...";
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

	private IEnumerator SpawnPlayer()
	{
		while(!npcs_generated)
			yield return new WaitForSeconds(wait_time);
		yield return new WaitForEndOfFrame();
		ls.current_loading_phase = "Spawning Player...";
		GameObject.FindGameObjectWithTag("PlayerSpawn").GetComponent<SpawnCharacter>().SpawnPlayer(mt);
		player_spawned = true;
		InitialiseFlares();
	}

	private void InitialiseFlares()
	{
		GameObject[] flares = GameObject.FindGameObjectsWithTag("Flare");
		foreach(GameObject flare in flares)
		{
			flare.GetComponent<FlareController>().Initialise();
		}
	}

	private int CountMinigames()
	{
		GameObject[] events = GameObject.FindGameObjectsWithTag("Event");
		return events.Length;
	}

	//TODO: remove all broken engineer panels or fixed ones based on if it is an engineer level
	private IEnumerator CheckDisableEngineering()
	{
		while(!floor_calculated)
			yield return new WaitForSeconds(wait_time);
		yield return new WaitForEndOfFrame();
		ls.current_loading_phase = "Disabling Unused Events...";
		UnityEngine.Debug.Log("Level manager mission type:" + mt);
		if(mt == MissionType.ENGINEERING) yield break;
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
							tform.GetComponent<Event>().EventNotNeeded();
							//tform.tag = "Untagged";
						}
					}
				}
			}
		}
		engineering_disabled = true;
	}

	private IEnumerator CheckDisableOxygen()
	{
		while(!floor_calculated)
			yield return new WaitForSeconds(wait_time);
		yield return new WaitForEndOfFrame();
		if(mt == MissionType.OXYGEN) yield break;
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
							tform.GetComponent<Event>().EventNotNeeded();
							//tform.tag = "Untagged";
						}
					}
				}
			}
		}
		oxygen_disabled = true;
	}

	private IEnumerator RandomiseEvents ()
	{
		while(!player_spawned)
			yield return new WaitForSeconds(wait_time);
		yield return new WaitForEndOfFrame();
		ls.current_loading_phase = "Randomising Events...";
		int x = CountMinigames();
		UnityEngine.Debug.Log ("Minigames to choose from:" + x);

		List<int> the_chosen_ones = new List<int> ();
		int r;
		switch (difficulty) {
		case Difficulty.Easy:
			r = Random.Range(2, 4);
			if(r > x)
				r = x;
			the_chosen_ones = ChooseRandomNumbers(r, x);
			break;
		case Difficulty.Medium:
			r = Random.Range(3, 5);
			if(r > x)
				r = x;
			the_chosen_ones = ChooseRandomNumbers(r, x);
			break;
		case Difficulty.Hard:
			r = Random.Range(3, 5);
			if(r > x)
				r = x;
			the_chosen_ones = ChooseRandomNumbers(r, x);
			break;
		case Difficulty.Insane:
			r = Random.Range(3, 6);
			if(r > x)
				r = x;
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
				UnityEngine.Debug.Log(events[i].name + " " + events[i].tag);
				try
				{
					events[i].GetComponent<Event>().EventNeeded();
				}
				catch{}
				continue;
			}

			if(mt == MissionType.ILLNESS)
				Destroy(events[i]);
			try
			{
				events[i].GetComponent<Event>().EventNotNeeded();
			}
			catch {}
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
		yield return new WaitForEndOfFrame();
		
		ls.current_loading_phase = "Generating AI Paths...";
		GameObject[] agents = GameObject.FindGameObjectsWithTag("AI");
		UnityEngine.Debug.Log("num of agents:" + agents.Length);
		foreach(GameObject agent in agents)
		{
			try
			{
				agent.GetComponent<AIMovement>().Initialise();
			}
			catch {}
		}

		int minigames = CountMinigames();
		UnityEngine.Debug.Log("level loaded");
		GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().LevelLoaded(minigames);
		UnityEngine.Debug.Log("done");
		Loading = false;
	}
	
	private IEnumerator DisableInvalidNodes()
	{
		while(!oxygen_disabled && !engineering_disabled)
			yield return new WaitForSeconds(wait_time);
		
		yield return new WaitForEndOfFrame();
		ls.current_loading_phase = "Disabling Invalid AI Nodes...";
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RoomType
{
	MEDIC,
	ENGINEER,
	FLIGHT_DECK,
	SLEEPING,
	OXYGEN,
	HALLWAY
}

public enum RoomSize
{
	SMALL,
	MEDIUM,
	LARGE
}

public class RoomManager : MonoBehaviour
{

    public bool safe = false;
    public bool wideRoom = false;
	public RoomType type;
	public RoomSize size;
    private List<GameObject> neighbours = new List<GameObject>();
    private List<GameObject> doors;
	public GameObject wall_spawn;

	void Awake()
	{
		if(wall_spawn)
		{
			Transform[] tforms = gameObject.GetComponentsInChildren<Transform>();
			foreach(Transform tform in tforms)
			{
				if(tform.CompareTag("Wall") && tform.name.Equals("Wall_Half"))
				{
					GameObject temp = (GameObject)Instantiate(wall_spawn, tform.position, tform.rotation);
					temp.transform.SetParent(tform.parent);
					Destroy(tform.gameObject);
				}
			}
		}
	}

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Rotate()
    {
		GetLocalDoors();
		foreach (GameObject door in doors)
		{
			DoorManager dm = door.GetComponent<DoorManager>();
			if(dm)
				dm.SetAccess(false);
		}
        if(wideRoom) transform.Rotate(new Vector3(0, 180, 0));
        else transform.Rotate(new Vector3(0, 90, 0));
    }

    void GetNeighbours()
    {
        for (int i = 0; i < doors.Count; i++)
        {
//			Collider[] cols = Physics.OverlapSphere(doors[i].transform.position, 0.5f);
//			foreach(Collider col in cols)
//			{
//				Debug.Log("door collided with: " + col.name + " " + col.tag);
//			}



            GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door");
			bool access= false;
            for (int j = 0; j < allDoors.Length; j++)
            {
                if (doors[i] != allDoors[j] && Vector3.Distance(doors[i].transform.position, allDoors[j].transform.position) < 0.5f)
                {
					if(allDoors[j].transform.root.GetComponent<RoomManager>().safe)
					{
						//neighbours.Add(allDoors[j].transform.root.gameObject);
						DoorManager dm = allDoors[j].GetComponent<DoorManager>();
						if(dm)
							dm.SetAccess(true);
						safe = true;
						DoorManager this_dm = doors[i].GetComponent<DoorManager>();
						if(this_dm)
							this_dm.SetAccess(true);
						access = true;
					}
//					try
//					{
//						allDoors[j].GetComponent<DoorManager>().SetAccess(true);
//						doors[i].GetComponent<DoorManager>().SetAccess(true);
//					}
//					catch {}
				}
            }
			if(!access)
			{
				DoorManager this_dm = doors[i].GetComponent<DoorManager>();
				if(this_dm)
					this_dm.SetAccess(false);
			}
        }
    }

    public void SafeNeighbourCheck()
    {
		GetLocalDoors();
        GetNeighbours();
//        for (int i = 0; i < neighbours.Count; i++)
//        {
//            if (neighbours[i].GetComponent<RoomManager>().safe)
//			{
//
//
//                safe = true;
//			}
//        }
    }

	private void GetLocalDoors()
	{
		doors = new List<GameObject>();
		Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
		foreach(Transform transform in transforms)
		{
			if(transform.CompareTag("Door"))
				doors.Add(transform.gameObject);
		}
	}
}

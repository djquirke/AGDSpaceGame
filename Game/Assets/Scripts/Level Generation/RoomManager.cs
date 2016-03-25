using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RoomType
{
	MEDIC,
	ENGINEER,
	FLIGHT_DECK,
	SLEEPING,
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
        if(wideRoom) transform.Rotate(new Vector3(0, 180, 0));
        else transform.Rotate(new Vector3(0, 90, 0));
    }

    void GetNeighbours()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door");
            for (int j = 0; j < allDoors.Length; j++)
            {
                if (doors[i] != allDoors[j] && Vector3.Distance(doors[i].transform.position, allDoors[j].transform.position) < 1.0f)
                {
					neighbours.Add(allDoors[j].transform.root.gameObject);
					try
					{
						allDoors[j].GetComponent<DoorManager>().SetAccess(true);
					}
					catch {}
				}
            }
        }
    }

    public void SafeNeighbourCheck()
    {
		GetLocalDoors();
        GetNeighbours();
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].GetComponent<RoomManager>().safe)
                safe = true;
        }
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

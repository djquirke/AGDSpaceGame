using UnityEngine;
using System.Collections;

public struct Exits
{
	public bool north, south, east, west;

	public Exits(bool north, bool south, bool east, bool west)
	{
		this.north = north;
		this.south = south;
		this.east = east;
		this.west = west;
	}
}

public class Tile : MonoBehaviour {

	private Exits flow_exits;
	public bool north, south, east, west;
	//public Transform camera;

	// Use this for initialization
	void Start () {
		//transform.position = new Vector3(camera.position.x, camera.position.y, camera.position.z + 10);
		//transform.LookAt(camera);
		//transform.Rotate(0, 180, 0);

		flow_exits = new Exits(north, south, east, west);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("north " + flow_exits.north + ", south " + flow_exits.south + ", east " + flow_exits.east + ", west " + flow_exits.west);
	}

	public void Initialise(Transform transform)
	{
		this.transform.position = transform.position;
	}

	public void MouseClick()
	{
		Debug.Log("north " + flow_exits.north + ", south " + flow_exits.south + ", east " + flow_exits.east + ", west " + flow_exits.west);
		transform.Rotate(0, 0, -90);
		AdjustExits();
	}

	void AdjustExits()
	{
		Exits temp = new Exits(false, false, false, false);
		if(flow_exits.north)
		{
			temp.east = true;
		}
		if(flow_exits.east)
		{
			temp.south = true;
		}
		if(flow_exits.south)
		{
			temp.west = true;
		}
		if(flow_exits.west)
		{
			temp.north = true;
		}

		flow_exits = temp;
	}
}

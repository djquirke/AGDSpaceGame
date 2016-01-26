using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	protected Exits flow_exits;
	public bool north, south, east, west;

	// Use this for initialization
	void Start () {
		flow_exits = new Exits(north, south, east, west);
	}

	public Exits Exit()
	{
		return flow_exits;
	}

	public void AdjustExits()
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

	public void setFlow(bool flow)
	{
		Image tile_col = gameObject.GetComponent<Image>();
		if(flow)
		{
			tile_col.color = new Color(0, 0.5f, 0);
		}
		else
		{
			tile_col.color = new Color(1, 1, 1);
		}
	}
}

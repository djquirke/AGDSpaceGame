using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Direction
{
	NORTH,
	EAST,
	SOUTH,
	WEST
}

class AStarNode
{
	public AStarNode parent;
	public int x, y;
	public float f, g, h;
}

public class BoardManager : MonoBehaviour {

	private List<List<GameObject>> tiles; //<width<height>>
	public GameObject start_tile, end_tile;
	private int board_width = 11, board_height = 11;
	public GameObject TileNE, TileNES, TileNESW, TileNS;

	// Use this for initialization
	void Start () {
		tiles = new List<List<GameObject>>();
		Initialise();
	}

	public void Initialise()
	{
		start_tile = (GameObject)Instantiate (start_tile);
		start_tile.GetComponent<TileStartEnd>().Initialise(0, UnityEngine.Random.Range(1, board_height - 2)); //set start tile to a random height
		end_tile = (GameObject)Instantiate (end_tile);
		end_tile.GetComponent<TileStartEnd> ().Initialise (board_width - 1, UnityEngine.Random.Range (1, board_height - 2));
		
		RectTransform cvs_rtransform = gameObject.GetComponent<RectTransform>();
		RectTransform tile_rtransform = start_tile.GetComponent<RectTransform>(); //get tile dimensions
		 
		for(int i = 0; i < board_width; i++)
		{
			List<GameObject> tempList = new List<GameObject>();
			for(int j = 0; j < board_height; j++)
			{
				GameObject new_tile;

				//add start tile to list
				if(i == 0 && j == start_tile.GetComponent<TileStartEnd>().Height())
				{
					start_tile.transform.SetParent(gameObject.transform, false);
					start_tile.transform.position = new Vector3(cvs_rtransform.rect.width / 2, cvs_rtransform.rect.height / 2);
					start_tile.GetComponent<TileStartEnd>().setPos(tile_rtransform);
					new_tile = start_tile;
					tempList.Add(new_tile);
					continue;
				}

				//add end tile to list
				if(i == board_width - 1 && j == end_tile.GetComponent<TileStartEnd>().Height())
				{
					end_tile.transform.SetParent(gameObject.transform, false);
					end_tile.transform.position = new Vector3(cvs_rtransform.rect.width / 2, cvs_rtransform.rect.height / 2);
					end_tile.GetComponent<TileStartEnd>().setPos(tile_rtransform);
					new_tile = end_tile;
					tempList.Add(new_tile);
					continue;
				}

				//add blank tiles to list
				if( i == 0 || i == board_width - 1 || j == 0 || j == board_height - 1)
				{
					new_tile = new GameObject();
					new_tile.transform.SetParent(gameObject.transform, false);
					new_tile.tag = "TileBlank";
					tempList.Add(new_tile);
					continue; 
				}

				//choose a random tile
				switch (UnityEngine.Random.Range(0, 3))
				{
				case 0:
					new_tile = (GameObject)Instantiate(TileNE);
					break;
				case 1:
					new_tile = (GameObject)Instantiate(TileNES);
					break;
				case 2:
					new_tile = (GameObject)Instantiate(TileNESW);
					break;
				default:
					new_tile = (GameObject)Instantiate(TileNS);
					break;
				}

				new_tile.transform.SetParent(gameObject.transform, false); //make child of canvas
				new_tile.transform.position = 
					new Vector3((cvs_rtransform.rect.width / 2) - ((board_width - 1) / 2 - i) * tile_rtransform.rect.width / 2,
					            (cvs_rtransform.rect.height / 2) + ((board_height - 1) / 2 - j) * tile_rtransform.rect.height / 2);
				Debug.Log("tile width: " + tile_rtransform.rect.size.x + ", tile height: " + tile_rtransform.rect.size.y);

				tempList.Add(new_tile);
			}
			tiles.Add(tempList);
		}
	}

	public void CheckFlow()
	{
		ResetTileColours();
		CheckFlowAStar();
		//int start_y = start_tile.GetComponent<TileStartEnd>().Height ();

		//CheckFlowRecursion (0, start_y);
	}

	void CheckFlowAStar()
	{
		List<AStarNode> open_nodes = new List<AStarNode>();
		List<AStarNode> closed_nodes = new List<AStarNode>();

		AStarNode goal = new AStarNode();
		goal.x = board_width - 1;
		goal.y = end_tile.GetComponent<TileStartEnd>().Height();

		AStarNode start = new AStarNode();
		start.x = 0;
		start.y = start_tile.GetComponent<TileStartEnd>().Height();
		start.h = GetNodeDistance(start, goal);
		start.g = 0;
		start.f = start.g + start.h;

		open_nodes.Add(start);
		bool atGoal = false;
		//Debug.Log(start.x.ToString() + " " + start.y.ToString() + " " + start.g.ToString() + " " + start.f.ToString() + " " + start.h.ToString());
		//Debug.Log(goal.x.ToString() + " " + goal.y.ToString());

		while (open_nodes.Count > 0)
		{
			Debug.Log("open node count: " + open_nodes.Count.ToString());

			AStarNode mvp = getMostPromisingNode(open_nodes);

			Debug.Log ("xpos of mvp: " + mvp.x + " ypos of mvp: " + mvp.y);

			open_nodes.Remove(mvp);

			List<AStarNode> successors = getNodeSuccessors(mvp);

			Debug.Log("successor count: " + successors.Count.ToString());

			foreach(AStarNode node in successors)
			{
				Debug.Log("successor xpos: " + node.x + " successor ypos: " + node.y);

				if(AtSamePos(node, goal))
				{
					goal.parent = mvp;
					atGoal = true;
					break;
				}
				if(AtSamePos(node, start))
				{
					continue;
				}

				node.g = mvp.g + 1;
				node.h = GetNodeDistance(node, goal);
				node.f = node.g + node.h;

				if(BetterNodeAlreadyInList(node, open_nodes))
					continue;
				if(BetterNodeAlreadyInList(node, closed_nodes))
					continue;

				open_nodes.Add(node);
			}

			if(atGoal)
				break;
			closed_nodes.Add(mvp);
		}

		if(atGoal)
		{
			TraversePath(goal);
		}
	}

	void TraversePath(AStarNode node)
	{
		Tile this_node = getTile(node.x, node.y);
		this_node.setFlow(true);

		if(node.parent == null)
		{
			return;
		}
		TraversePath(node.parent);
	}

	float GetNodeDistance(AStarNode start, AStarNode end)
	{
		return (Mathf.Pow((end.x - start.x), 2) + Mathf.Pow((end.y - start.y), 2));
	}

	AStarNode getMostPromisingNode(List<AStarNode> avail_nodes)
	{
		AStarNode mvp = new AStarNode();
		float smallest_f = UnityEngine.Mathf.Infinity;
		foreach(AStarNode node in avail_nodes)
		{
			if(node.f < smallest_f)
			{
				smallest_f = node.f;
				mvp = node;
			}
		}
		return mvp;
	}
	
	List<AStarNode> getNodeSuccessors(AStarNode node)
	{
		List<AStarNode> successors = new List<AStarNode>();
		Tile cur_tile_exits = getTile(node.x, node.y);
		if(cur_tile_exits.north)
		{
			int[] nextcoords = getNextCoords(node.x, node.y, Direction.NORTH);
			if(!BlankTile(nextcoords[0], nextcoords[1]))
			{
				Tile next_tile = getTile(nextcoords[0], nextcoords[1]);
				if(next_tile.hasExitOppositeTo(Direction.NORTH))
				{
					AStarNode successor = new AStarNode();
					successor.x = nextcoords[0];
					successor.y = nextcoords[1];
					successor.parent = node;
					successors.Add(successor);
				}
			}


		}
		if(cur_tile_exits.east)
		{
			int[] nextcoords = getNextCoords(node.x, node.y, Direction.EAST);
			if(!BlankTile(nextcoords[0], nextcoords[1]))
			{
				Tile next_tile = getTile(nextcoords[0], nextcoords[1]);
				if(next_tile.hasExitOppositeTo(Direction.EAST))
				{
					AStarNode successor = new AStarNode();
					successor.x = nextcoords[0];
					successor.y = nextcoords[1];
					successor.parent = node;
					successors.Add(successor);
				}
			}


		}
		if(cur_tile_exits.south)
		{
			int[] nextcoords = getNextCoords(node.x, node.y, Direction.SOUTH);
			if(!BlankTile(nextcoords[0], nextcoords[1]))
			{
				Tile next_tile = getTile(nextcoords[0], nextcoords[1]);
				if(next_tile.hasExitOppositeTo(Direction.SOUTH))
				{
					AStarNode successor = new AStarNode();
					successor.x = nextcoords[0];
					successor.y = nextcoords[1];
					successor.parent = node;
					successors.Add(successor);
				}
			}


		}
		if(cur_tile_exits.west)
		{
			int[] nextcoords = getNextCoords(node.x, node.y, Direction.WEST);
			if(!BlankTile(nextcoords[0], nextcoords[1]))
			{
				Tile next_tile = getTile(nextcoords[0], nextcoords[1]);
				if(next_tile.hasExitOppositeTo(Direction.WEST))
				{
					AStarNode successor = new AStarNode();
					successor.x = nextcoords[0];
					successor.y = nextcoords[1];
					successor.parent = node;
					successors.Add(successor);
				}
			}


		}

//		foreach(Direction dir in Enum.GetValues(typeof(Direction)))
//		{
//			int[] nextcoords = getNextCoords(node.x, node.y, dir);
//
//			if(BlankTile(nextcoords[0], nextcoords[1]))
//			{
//				continue;
//			}
//
//			Tile next_tile = getNextExits(nextcoords[0], nextcoords[1]);
//			if(next_tile.hasExitOppositeTo(dir))
//			{
//				AStarNode successor = new AStarNode();
//				successor.x = nextcoords[0];
//				successor.y = nextcoords[1];
//				successor.parent = node;
//				successors.Add(successor);
//			}
//		}
		return successors;
	}

	bool AtSamePos(AStarNode node1, AStarNode node2)
	{
		return (node1.x == node2.x && node1.y == node2.y);
	}

	bool BetterNodeAlreadyInList(AStarNode node, List<AStarNode> list)
	{
		bool ret = false;

		foreach(AStarNode anode in list)
		{
			if(AtSamePos(node, anode) && anode.f < node.f)
			{
				ret = true;
				break;
			}
		}

		return ret;
	}

	bool BlankTile(int pos_x, int pos_y)
	{
		if(pos_x < 0 || pos_y < 0 || pos_x >= board_width || pos_y >= board_height)
			return true;
		return tiles[pos_x][pos_y].CompareTag("TileBlank");
	}

	bool CheckEndTile(int cur_x, int cur_y)
	{
		return (cur_x == board_width && cur_y == GameObject.FindGameObjectWithTag ("TileEnd").GetComponent<TileStartEnd> ().Height ());
	}

	int[] getNextCoords(int cur_x, int cur_y, Direction dir)
	{
		int next_x, next_y;
		
		switch (dir)
		{
		case Direction.NORTH:
			next_x = cur_x;
			next_y = --cur_y;
			break;
		case Direction.EAST:
			next_x = ++cur_x;
			next_y = cur_y;
			break;
		case Direction.SOUTH:
			next_x = cur_x;
			next_y = ++cur_y;
			break;
		case Direction.WEST:
			next_x = --cur_x;
			next_y = cur_y;
			break;
		default:
			next_x = cur_x;
			next_y = cur_y;
			break;
		}

		return new int[2]{next_x, next_y};

	}

	Tile getTile(int x, int y)
	{
		return tiles [x] [y].GetComponent<Tile> ();
	}

	void ResetTileColours()
	{
		GameObject.FindGameObjectWithTag("TileStart").GetComponent<TileStartEnd>().setFlow(false);
		GameObject.FindGameObjectWithTag("TileEnd").GetComponent<TileStartEnd>().setFlow(false);
		for(int i = 1; i < board_width - 1; i++)
		{
			for(int j = 1; j < board_height - 1; j++)
			{
				tiles[i][j].GetComponent<Tile>().setFlow(false);
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction
{
	NORTH,
	EAST,
	SOUTH,
	WEST
}

public class BoardManager : MonoBehaviour {

	private List<List<GameObject>> tiles; //<width<height>>
	public GameObject start_tile, end_tile;
	private int board_width = 11, board_height = 11;
	public GameObject TileNE, TileNES, TileNESW, TileNS;
	//just one path to connect?


	// Use this for initialization
	void Start () {
		tiles = new List<List<GameObject>>();
		Initialise();
	}

	public void Initialise()
	{
		start_tile = (GameObject)Instantiate (start_tile);
		start_tile.GetComponent<TileStartEnd>().Initialise(0, Random.Range(1, board_height - 2)); //set start tile to a random height
		end_tile = (GameObject)Instantiate (end_tile);
		end_tile.GetComponent<TileStartEnd> ().Initialise (board_width - 1, Random.Range (1, board_height - 2));
		
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
				switch (Random.Range(0, 3))
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
					new Vector3((cvs_rtransform.rect.width / 2) - (5 - i) * tile_rtransform.rect.width / 2,
					            (cvs_rtransform.rect.height / 2) + (5 - j) * tile_rtransform.rect.height / 2);
				Debug.Log("tile width: " + tile_rtransform.rect.size.x + ", tile height: " + tile_rtransform.rect.size.y);

				tempList.Add(new_tile);
			}
			tiles.Add(tempList);
		}
	}

	public void CheckFlow()
	{
		int start_y = start_tile.GetComponent<TileStartEnd>().Height ();

		CheckFlowRecursion (0, start_y);

//		Exits exits1 = tiles[1][1].GetComponent<Tile>().Exit();
//		Exits exits2 = tiles[2][1].GetComponent<Tile>().Exit();
//		if(exits1.east && exits2.west)
//		{
//			tiles[1][1].GetComponent<Tile>().setFlow(true);
//			tiles[2][1].GetComponent<Tile>().setFlow(true);
//		}
//		else
//		{
//			tiles[1][1].GetComponent<Tile>().setFlow(false);
//			tiles[2][1].GetComponent<Tile>().setFlow(false);
//		}
	}

	bool CheckFlowRecursion(int cur_x, int cur_y)
	{
		bool running = true;
		
		while (running)
		{
			//running = check to see if at end tile
			if(CheckEndTile(cur_x, cur_y))
				return true;
			Debug.Log(tiles[cur_x][cur_y].tag);
			if(BlankTile(cur_x, cur_y))
				return false;

			Exits cur_exits = tiles[cur_x][cur_y].GetComponent<Tile>().Exit();

			if(cur_exits.north)
			{
				int[] next_tile_pos = getNextCoords(cur_x, cur_y, Direction.NORTH);

				if(BlankTile(next_tile_pos[0], next_tile_pos[1]))
					break;

				Exits next_tile_exits = getNextExits(next_tile_pos[0], next_tile_pos[1]);

				if(next_tile_exits.south)
				{
					tiles[cur_x][cur_y].GetComponent<Tile>().setFlow(true);
					tiles[next_tile_pos[0]][next_tile_pos[1]].GetComponent<Tile>().setFlow(true);
				}
			}
			if(cur_exits.east)
			{
				int[] next_tile_pos = getNextCoords(cur_x, cur_y, Direction.EAST);
				
				if(BlankTile(next_tile_pos[0], next_tile_pos[1]))
					break;
				
				Exits next_tile_exits = getNextExits(next_tile_pos[0], next_tile_pos[1]);
				
				if(next_tile_exits.west)
				{
					tiles[cur_x][cur_y].GetComponent<Tile>().setFlow(true);
					tiles[next_tile_pos[0]][next_tile_pos[1]].GetComponent<Tile>().setFlow(true);
				}
			}
			if(cur_exits.south)
			{
				
			}
			if(cur_exits.west)
			{
				
			}


			running = false;
		}
		return true;
	}

	bool BlankTile(int pos_x, int pos_y)
	{
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
			next_y = cur_y--;
			break;
		case Direction.EAST:
			next_x = cur_x++;
			next_y = cur_y;
			break;
		case Direction.SOUTH:
			next_x = cur_x;
			next_y = cur_y++;
			break;
		case Direction.WEST:
			next_x = cur_x--;
			next_y = cur_y;
			break;
		default:
			next_x = cur_x;
			next_y = cur_y;
			break;
		}

		return new int[2]{next_x, next_y};

	}

	Exits getNextExits(int next_x, int next_y)
	{
		return tiles [next_x] [next_y].GetComponent<Tile> ().Exit ();
	}
}

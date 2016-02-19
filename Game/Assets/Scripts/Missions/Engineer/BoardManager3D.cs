using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PipeGame
{
	public enum Direction
	{
		NORTH,
		EAST,
		SOUTH,
		WEST
	}

	public class BoardManager3D : MonoBehaviour {

		private List<List<GameObject>> tiles; //<width<height>>
		private int board_width = 11, board_height = 11;
		public GameObject TileNE, TileNES, TileNESW, TileNS, start, end;

		// Use this for initialization
		void Start () {
			tiles = new List<List<GameObject>>();
			Initialise();
		}

		public void Initialise()
		{
			start = (GameObject)Instantiate (start);
			start.GetComponent<Tile3D>().Initialise(0, UnityEngine.Random.Range(1, board_height - 2), false); //set start tile to a random height
			end = (GameObject)Instantiate(end);
			end.GetComponent<Tile3D>().Initialise (board_width - 1, UnityEngine.Random.Range (1, board_height - 2), false);

			Camera cam = gameObject.GetComponent<Camera>();
			float cam_height = 2f * cam.orthographicSize;
			float cam_width = cam_height * cam.aspect;
			Debug.Log(cam_width + " " + cam_height);
			Renderer tile_size = start.GetComponent<Renderer>(); //get tile dimensions
			
			for(int i = 0; i < board_width; i++)
			{
				List<GameObject> tempList = new List<GameObject>();
				for(int j = 0; j < board_height; j++)
				{
					GameObject new_tile = null;

					//add start tile to list
					if(i == 0 && j == start.GetComponent<Tile3D>().Y())
					{
						new_tile = start;
//						start.transform.SetParent(gameObject.transform, false);
//						start.transform.position = new Vector3(cam_width / 2, cam_height / 2, 5); //TODO: fix pos here
//						//start.GetComponent<Tile3D>().setPos(tile_transform);
//						new_tile = start;
//						tempList.Add(new_tile);
//						continue;
					}
					
					//add end tile to list
					if(i == board_width - 1 && j == end.GetComponent<Tile3D>().Y())
					{
						new_tile = end;
//						end.transform.SetParent(gameObject.transform, false);
//						end.transform.position = new Vector3(cam_width / 2, cam_height / 2, 5); //TODO: fix pos here
//						//end.GetComponent<tile>().setPos(tile_transform);
//						new_tile = end;
//						tempList.Add(new_tile);
//						continue;
					}
					
					//add blank tiles to list
					if( i == 0 || i == board_width - 1 || j == 0 || j == board_height - 1 && new_tile == null)
					{
						new_tile = new GameObject();
						new_tile.transform.SetParent(gameObject.transform, false);
						new_tile.tag = "TileBlank";
						tempList.Add(new_tile);
						continue; 
					}

					if(new_tile == null)
					{
						//choose a random tile
						switch (UnityEngine.Random.Range(0, 4))
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
					}
					
					new_tile.transform.SetParent(gameObject.transform, false); //make child of camera
					new_tile.transform.position = new Vector3(cam_width / 4, cam_height / 4, 50);
						//new Vector3((cam_width / 2) - ((board_width - 1) / 2 - i) * tile_size.bounds.size.x / 2,
						//            (cam_height / 2) + ((board_height - 1) / 2 - j) * tile_size.bounds.size.y / 2, 50);
					Debug.Log("tile width: " + tile_size.bounds.size.x + ", tile height: " + tile_size.bounds.size.y);
					//new_tile = new GameObject();
					tempList.Add(new_tile);
				}
				tiles.Add(tempList);
			}
		}

		public void CheckFlow()
		{
			ResetTileColours();
			AStarPipe astar = new AStarPipe();
			astar.Run(start, end, board_width, this);
		}
		
		public bool BlankTile(int pos_x, int pos_y)
		{
			if(pos_x < 0 || pos_y < 0 || pos_x >= board_width || pos_y >= board_height)
				return true;
			return tiles[pos_x][pos_y].CompareTag("TileBlank");
		}
		
		bool CheckEndTile(int cur_x, int cur_y)
		{
			return (cur_x == board_width && cur_y == GameObject.FindGameObjectWithTag ("TileEnd").GetComponent<Tile3D> ().Y ());
		}
		
		public int[] getNextCoords(int cur_x, int cur_y, Direction dir)
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
		
		public Tile3D getTile(int x, int y) { return tiles[x][y].GetComponent<Tile3D>(); }
		
		void ResetTileColours()
		{
			//GameObject.FindGameObjectWithTag("TileStart").GetComponent<Tile3D>().setFlow(false);
			//GameObject.FindGameObjectWithTag("TileEnd").GetComponent<Tile3D>().setFlow(false);
			for(int i = 0; i < board_width; i++)
			{
				for(int j = 0; j < board_height; j++)
				{
					if(tiles[i][j].CompareTag("TileBlank")) continue;

					tiles[i][j].GetComponent<Tile3D>().setFlow(false);
				}
			}
		}
	}

}
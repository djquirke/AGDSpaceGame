using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

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
		private int board_width = 7, board_height = 7;
		public GameObject TileNE, TileNES, TileNESW, TileNS, TileNull, start, end, blank, corner, stage9, stage7, stage5;
		private int depth = 15;
		private Vector3 cam_pos;
		private Stopwatch completion_pause;
		private float completion_pause_time = 2000;
		private bool game_complete;
		private Event event_;

		void Update()
		{
			if(game_complete)
			{
				if(completion_pause.ElapsedMilliseconds >= completion_pause_time)
				{
					completion_pause.Reset();
					GameObject.FindGameObjectWithTag("PipeStage").GetComponent<PipeMiniGameAnimationController>().closeMiniGame();
				}
			}
		}

		public void Initialise(Difficulty difficulty, Event event_)
		{
			this.event_ = new PipeMinigame();
			this.event_ = event_;

			tiles = new List<List<GameObject>>();
			GetCameraDims();
			SpawnBoard(difficulty);

			start = (GameObject)Instantiate (start);
			start.GetComponent<Tile3D>().Initialise(0, UnityEngine.Random.Range(1, board_height - 2)); //set start tile to a random height
			end = (GameObject)Instantiate(end);
			end.GetComponent<Tile3D>().Initialise (board_width - 1, UnityEngine.Random.Range (1, board_height - 2));

			GameObject temp = (GameObject)Instantiate(TileNESW);
			var tile_size = temp.GetComponent<MeshFilter>().mesh.bounds; //get tile dimensions
			float tile_width = (tile_size.max.x - tile_size.min.x);
			Destroy(temp);

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
						new_tile.transform.SetParent(gameObject.transform, false);
						new_tile.transform.position = new Vector3(cam_pos.x - ((board_width ) / 2 - i) * tile_width,
						                                          cam_pos.y - ((board_height ) / 2 - j) * tile_width,
						                                          depth);
						tempList.Add(new_tile);
						continue;
					}
					
					//add end tile to list
					if(i == board_width - 1 && j == end.GetComponent<Tile3D>().Y())
					{
						new_tile = end;
						new_tile.transform.SetParent(gameObject.transform, false);
						new_tile.transform.position = new Vector3(cam_pos.x - ((board_width ) / 2 - i) * tile_width,
						                                          cam_pos.y - ((board_height ) / 2 - j) * tile_width,
						                                          depth);
						tempList.Add(new_tile);
						continue;
					}
					
					//add blank tiles to list
					if( i == 0 || i == board_width - 1 || j == 0 || j == board_height - 1)
					{
						if(i == 0 && j == 0)
						{
							new_tile = (GameObject)Instantiate(corner);
							new_tile.transform.SetParent(gameObject.transform, false);
							new_tile.transform.position = new Vector3(cam_pos.x - ((board_width ) / 2 - i) * tile_width,
							                                          cam_pos.y - ((board_height ) / 2 - j) * tile_width,
							                                          depth);
							new_tile.tag = "TileBlank";
							new_tile.transform.Rotate(0, 0, 180);
							tempList.Add(new_tile);
							continue;
						}
						else if(i == 0 && j == board_height - 1)
						{
							new_tile = (GameObject)Instantiate(corner);
							new_tile.transform.SetParent(gameObject.transform, false);
							new_tile.transform.position = new Vector3(cam_pos.x - ((board_width ) / 2 - i) * tile_width,
							                                          cam_pos.y - ((board_height ) / 2 - j) * tile_width,
							                                          depth);
							new_tile.tag = "TileBlank";
							new_tile.transform.Rotate(0, 0, -90);
							tempList.Add(new_tile);
							continue;
						}
						else if(i == board_width - 1 && j == 0)
						{
							new_tile = (GameObject)Instantiate(corner);
							new_tile.transform.SetParent(gameObject.transform, false);
							new_tile.transform.position = new Vector3(cam_pos.x - ((board_width ) / 2 - i) * tile_width,
							                                          cam_pos.y - ((board_height ) / 2 - j) * tile_width,
							                                          depth);
							new_tile.tag = "TileBlank";
							new_tile.transform.Rotate(0, 0, 90);
							tempList.Add(new_tile);
							continue;

						}
						else if(i == board_width - 1 && j == board_height - 1)
						{
							new_tile = (GameObject)Instantiate(corner);
							new_tile.transform.SetParent(gameObject.transform, false);
							new_tile.transform.position = new Vector3(cam_pos.x - ((board_width ) / 2 - i) * tile_width,
							                                          cam_pos.y - ((board_height ) / 2 - j) * tile_width,
							                                          depth);
							new_tile.tag = "TileBlank";
							tempList.Add(new_tile);
							continue;

						}


						new_tile = (GameObject)Instantiate(blank);//new GameObject();
						new_tile.transform.SetParent(gameObject.transform, false);
						new_tile.transform.position = new Vector3(cam_pos.x - ((board_width ) / 2 - i) * tile_width,
						                                          cam_pos.y - ((board_height ) / 2 - j) * tile_width,
						                                          depth);
						new_tile.tag = "TileBlank";

						if(i == 0) new_tile.transform.Rotate(0, 0, 180);
						else if(j == 0) new_tile.transform.Rotate(0, 0, 90);
						else if(j == board_height - 1) new_tile.transform.Rotate(0, 0, -90);

						tempList.Add(new_tile);
						continue;
					}

					//choose a random tile
					switch (UnityEngine.Random.Range(0, 5))
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
//					case 3:
//						new_tile = (GameObject)Instantiate(TileNull);
//						break;
					default:
						new_tile = (GameObject)Instantiate(TileNS);
						break;
					}
					
					new_tile.transform.SetParent(gameObject.transform, false); //make child of camera
					new_tile.transform.position = new Vector3(cam_pos.x - ((board_width ) / 2 - i) * tile_width,
					                                          cam_pos.y - ((board_height ) / 2 - j) * tile_width,
					                                          depth);
					new_tile.GetComponent<Tile3D>().Initialise(i, j);
					tempList.Add(new_tile);
				}
				tiles.Add(tempList);
			}
		}

		public void CheckFlow()
		{
			ResetTileColours();
			AStarPipe astar = new AStarPipe();
			if(astar.Run(start, end, board_width, this))
			{
				game_complete = true;
				completion_pause = new Stopwatch();
				completion_pause.Start();
			}

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
				next_y = ++cur_y;
				break;
			case Direction.EAST:
				next_x = ++cur_x;
				next_y = cur_y;
				break;
			case Direction.SOUTH:
				next_x = cur_x;
				next_y = --cur_y;
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
			for(int i = 1; i < board_width - 1; i++)
			{
				for(int j = 1; j < board_height - 1; j++)
				{
					if(tiles[i][j].CompareTag("TileBlank")) continue;

					tiles[i][j].GetComponent<Tile3D>().setFlow(false);
				}
			}
		}

        internal void FlowFound()
        {
            for (int i = 1; i < board_width - 1; i++)
            {
                for (int j = 1; j < board_height - 1; j++)
                {
                    tiles[i][j].GetComponent<Tile3D>().FlowActive();
                }
            }
        }

		private void SpawnBoard(Difficulty difficulty)
		{
			GameObject bg = null;
			
			if(difficulty == Difficulty.Hard || difficulty == Difficulty.Insane)
			{
				bg = (GameObject)Instantiate(stage9);
				board_width = 11;
				board_height = 11;
			}
			else if(difficulty == Difficulty.Medium)
			{
				bg = (GameObject)Instantiate(stage7);
				board_width = 9;
				board_height = 9;
			}
			else if(difficulty == Difficulty.Easy)
			{
				bg = (GameObject)Instantiate(stage5);
				board_width = 7;
				board_height = 7;
			}

			bg.transform.SetParent(gameObject.transform, false);
			bg.transform.position = new Vector3(cam_pos.x, cam_pos.y, depth);
		}

		private void GetCameraDims()
		{
			Camera cam = gameObject.GetComponent<Camera>();
			float cam_height = 2f * cam.orthographicSize;
			float cam_width = cam_height * cam.aspect;
			cam_pos = new Vector3();
			cam_pos = cam.transform.position;
		}

		public void DoorsClosed()
		{
			//GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().MinigameComplete();
			event_.Success();
			Destroy(this.gameObject);
		}
	}

}
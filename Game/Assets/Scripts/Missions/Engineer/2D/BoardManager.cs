//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//
//namespace PipeGame
//{
//	public enum Direction
//	{
//		NORTH,
//		EAST,
//		SOUTH,
//		WEST
//	}
//	
//	public class BoardManager : MonoBehaviour {
//		
//		private List<List<GameObject>> tiles; //<width<height>>
//		public GameObject start_tile, end_tile;
//		private int board_width = 11, board_height = 11;
//		public GameObject TileNE, TileNES, TileNESW, TileNS;
//		
//		// Use this for initialization
//		void Start () {
//			tiles = new List<List<GameObject>>();
//			Initialise();
//		}
//		
//		public void Initialise()
//		{
//			start_tile = (GameObject)Instantiate (start_tile);
//			start_tile.GetComponent<TileStartEnd>().Initialise(0, UnityEngine.Random.Range(1, board_height - 2)); //set start tile to a random height
//			end_tile = (GameObject)Instantiate (end_tile);
//			end_tile.GetComponent<TileStartEnd> ().Initialise (board_width - 1, UnityEngine.Random.Range (1, board_height - 2));
//			
//			RectTransform cvs_rtransform = gameObject.GetComponent<RectTransform>();
//			RectTransform tile_rtransform = start_tile.GetComponent<RectTransform>(); //get tile dimensions
//			
//			for(int i = 0; i < board_width; i++)
//			{
//				List<GameObject> tempList = new List<GameObject>();
//				for(int j = 0; j < board_height; j++)
//				{
//					GameObject new_tile;
//					
//					//add start tile to list
//					if(i == 0 && j == start_tile.GetComponent<TileStartEnd>().Height())
//					{
//						start_tile.transform.SetParent(gameObject.transform, false);
//						start_tile.transform.position = new Vector3(cvs_rtransform.rect.width / 2, cvs_rtransform.rect.height / 2);
//						start_tile.GetComponent<TileStartEnd>().setPos(tile_rtransform);
//						new_tile = start_tile;
//						tempList.Add(new_tile);
//						continue;
//					}
//					
//					//add end tile to list
//					if(i == board_width - 1 && j == end_tile.GetComponent<TileStartEnd>().Height())
//					{
//						end_tile.transform.SetParent(gameObject.transform, false);
//						end_tile.transform.position = new Vector3(cvs_rtransform.rect.width / 2, cvs_rtransform.rect.height / 2);
//						end_tile.GetComponent<TileStartEnd>().setPos(tile_rtransform);
//						new_tile = end_tile;
//						tempList.Add(new_tile);
//						continue;
//					}
//					
//					//add blank tiles to list
//					if( i == 0 || i == board_width - 1 || j == 0 || j == board_height - 1)
//					{
//						new_tile = new GameObject();
//						new_tile.transform.SetParent(gameObject.transform, false);
//						new_tile.tag = "TileBlank";
//						tempList.Add(new_tile);
//						continue; 
//					}
//					
//					//choose a random tile
//					switch (UnityEngine.Random.Range(0, 4))
//					{
//					case 0:
//						new_tile = (GameObject)Instantiate(TileNE);
//						break;
//					case 1:
//						new_tile = (GameObject)Instantiate(TileNES);
//						break;
//					case 2:
//						new_tile = (GameObject)Instantiate(TileNESW);
//						break;
//					default:
//						new_tile = (GameObject)Instantiate(TileNS);
//						break;
//						break;
//					}
//					
//					new_tile.transform.SetParent(gameObject.transform, false); //make child of canvas
//					new_tile.transform.position = 
//						new Vector3((cvs_rtransform.rect.width / 2) - ((board_width - 1) / 2 - i) * tile_rtransform.rect.width / 2,
//						            (cvs_rtransform.rect.height / 2) + ((board_height - 1) / 2 - j) * tile_rtransform.rect.height / 2);
//					Debug.Log("tile width: " + tile_rtransform.rect.size.x + ", tile height: " + tile_rtransform.rect.size.y);
//					
//					tempList.Add(new_tile);
//				}
//				tiles.Add(tempList);
//			}
//		}
//		
//		public void CheckFlow()
//		{
//			ResetTileColours();
//			AStarPipe astar = new AStarPipe();
//			astar.Run(start_tile, end_tile, board_width, this);
//		}
//		
//		public bool BlankTile(int pos_x, int pos_y)
//		{
//			if(pos_x < 0 || pos_y < 0 || pos_x >= board_width || pos_y >= board_height)
//				return true;
//			return tiles[pos_x][pos_y].CompareTag("TileBlank");
//		}
//		
//		bool CheckEndTile(int cur_x, int cur_y)
//		{
//			return (cur_x == board_width && cur_y == GameObject.FindGameObjectWithTag ("TileEnd").GetComponent<TileStartEnd> ().Height ());
//		}
//		
//		public int[] getNextCoords(int cur_x, int cur_y, Direction dir)
//		{
//			int next_x, next_y;
//			
//			switch (dir)
//			{
//			case Direction.NORTH:
//				next_x = cur_x;
//				next_y = --cur_y;
//				break;
//			case Direction.EAST:
//				next_x = ++cur_x;
//				next_y = cur_y;
//				break;
//			case Direction.SOUTH:
//				next_x = cur_x;
//				next_y = ++cur_y;
//				break;
//			case Direction.WEST:
//				next_x = --cur_x;
//				next_y = cur_y;
//				break;
//			default:
//				next_x = cur_x;
//				next_y = cur_y;
//				break;
//			}
//
//			return new int[2]{next_x, next_y};
//			
//		}
//		
//		public Tile getTile(int x, int y) { return tiles[x][y].GetComponent<Tile>(); }
//		
//		void ResetTileColours()
//		{
//			GameObject.FindGameObjectWithTag("TileStart").GetComponent<TileStartEnd>().setFlow(false);
//			GameObject.FindGameObjectWithTag("TileEnd").GetComponent<TileStartEnd>().setFlow(false);
//			for(int i = 1; i < board_width - 1; i++)
//			{
//				for(int j = 1; j < board_height - 1; j++)
//				{
//					tiles[i][j].GetComponent<Tile>().setFlow(false);
//				}
//			}
//		}
//	}
//}

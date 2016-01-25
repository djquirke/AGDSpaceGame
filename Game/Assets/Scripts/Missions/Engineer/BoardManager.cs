using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		RectTransform cvs_rtransform = gameObject.GetComponent<RectTransform>();
		start_tile = (GameObject)Instantiate (start_tile);
		start_tile.GetComponent<TileStart>().Initialise(2);
		RectTransform tile_rtransform = start_tile.GetComponent<RectTransform>();

		start_tile.transform.SetParent(gameObject.transform, false);
		start_tile.transform.position = new Vector3(cvs_rtransform.rect.width / 2, cvs_rtransform.rect.height / 2);
		start_tile.GetComponent<TileStart>().setPos(tile_rtransform);
		 
		for(int i = 0; i < board_width; i++)
		{
			List<GameObject> tempList = new List<GameObject>();
			for(int j = 0; j < board_height; j++)
			{
				GameObject new_tile;

				if( i == 0 || i == 10 || j == 0 || j == 10)
				{
					new_tile = new GameObject();
					new_tile.transform.SetParent(gameObject.transform, false);
					tempList.Add(new_tile);
					continue; 
				}

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
		Exits exits1 = tiles[1][1].GetComponent<Tile>().Exit();
		Exits exits2 = tiles[2][1].GetComponent<Tile>().Exit();
		if(exits1.east && exits2.west)
		{
			tiles[1][1].GetComponent<Tile>().setFlow(true);
			tiles[2][1].GetComponent<Tile>().setFlow(true);
		}
		else
		{
			tiles[1][1].GetComponent<Tile>().setFlow(false);
			tiles[2][1].GetComponent<Tile>().setFlow(false);
		}
	}
}

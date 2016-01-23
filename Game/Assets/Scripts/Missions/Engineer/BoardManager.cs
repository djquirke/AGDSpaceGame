using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	private List<List<GameObject>> tiles; //<width<height>>
	private int board_width = 9, board_height = 9;
	public GameObject TileNE, TileNES, TileNESW, TileNS;
	//just one path to connect?


	// Use this for initialization
	void Start () {
		Initialise();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialise()
	{
		for(int i = 0; i < board_width; i++)
		{
			for(int j = 0; j < board_height; j++)
			{
				GameObject new_tile = (GameObject)Instantiate(TileNES, gameObject.transform.position, gameObject.transform.rotation);
				new_tile.transform.SetParent(gameObject.transform, false);
				RectTransform cvs_rtransform = gameObject.GetComponent<RectTransform>();
				RectTransform tile_rtransform = new_tile.GetComponent<RectTransform>();
				new_tile.transform.position = 
					new Vector3((cvs_rtransform.rect.width / 2) - (4 - i) * tile_rtransform.rect.width / 2,
					            (cvs_rtransform.rect.height / 2) + (4 - j) * tile_rtransform.rect.height / 2);
				Debug.Log("tile width: " + tile_rtransform.rect.width + ", tile height: " + tile_rtransform.rect.height);
			}
		}

	}
}

using UnityEngine;
using System.Collections;

public class TileStart : Tile {

	int startHeight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialise(int height)
	{
		startHeight = height;
	}

	public int Height()
	{
		return startHeight;
	}

	public void setPos(RectTransform tile_dims)
	{
		this.transform.position = 
			new Vector3(this.transform.position.x - 5 * tile_dims.rect.width / 2,
			            this.transform.position.y + (5 - startHeight) * tile_dims.rect.height / 2);

	}


}

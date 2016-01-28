using UnityEngine;
using System.Collections;

public class TileStartEnd : Tile {

	int height, width;

	public void Initialise(int x_pos, int y_pos)
	{
		height = y_pos;
		width = x_pos;
	}

	public int Height()
	{
		return height;
	}

	public void setPos(RectTransform tile_dims)
	{
		this.transform.position = 
			new Vector3(this.transform.position.x - (5 - width) * tile_dims.rect.width / 2,
			            this.transform.position.y + (5 - height) * tile_dims.rect.height / 2);

	}


}

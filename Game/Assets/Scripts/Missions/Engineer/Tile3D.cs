using UnityEngine;
using System.Collections;

namespace PipeGame
{
	public class Tile3D : MonoBehaviour {

		private int x, y;
		public bool north, south, east, west, isStart, isEnd, isRotatable;
		private bool visited = false, rotate = false;

		void Update()
		{
//			if(rotate)
//			{
//				var new_angle = transform.rotation.z - 90;
//				while (transform.eulerAngles.z < new_angle)
//				{
//					transform.eulerAngles.z = Mathf.MoveTowards(transform.eulerAngles.z, new_angle, 90 * Time.deltaTime);
//					yield;
//				}
//				rotate = false;
//			}
		}

		public void Initialise(int x, int y, bool rotatable)
		{
			this.x = x;
			this.y = y;
			this.isRotatable = rotatable;
		}

		public int Y() { return this.y; }
		public int X() { return this.x; }
		public bool Visited() { return this.visited; }

		void OnMouseUp()
		{
			Debug.Log("Click");
			if(isRotatable)
			{
				//rotate = true;
				transform.Rotate(0, -90, 0);
				AdjustExits();
				GameObject.FindGameObjectWithTag("PipeCamera").GetComponent<BoardManager3D>().CheckFlow(); //TODO: get this is work with parent
			}
		}

		public void AdjustExits()
		{
			bool tn = false, te = false, ts = false, tw = false;
			
			if(north) te = true;
			if(east) ts = true;
			if(south) tw = true;
			if(west) tn = true;
			
			north = tn;
			east = te;
			south = ts;
			west = tw;
		}

		//TODO: shade flowing pipes
		public void setFlow(bool flow)
		{
			visited = flow;
//			Image tile_col = gameObject.GetComponent<Image>();
//			if(flow) tile_col.color = new Color(0, 0.5f, 0);
//			else tile_col.color = new Color(1, 1, 1);
		}
		
		public bool hasExitOppositeTo(Direction dir)
		{
			bool ret = false;
			
			switch (dir)
			{
			case Direction.EAST:
				ret = west;
				break;
			case Direction.NORTH:
				ret = south;
				break;
			case Direction.SOUTH:
				ret = north;
				break;
			case Direction.WEST:
				ret = east;
				break;
			}
			
			return ret;
		}
	}
}

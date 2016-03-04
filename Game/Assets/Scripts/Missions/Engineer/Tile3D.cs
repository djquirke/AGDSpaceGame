using UnityEngine;
using System.Collections;

namespace PipeGame
{
	public class Tile3D : MonoBehaviour {

		private int x, y;
		public bool north, south, east, west, isStart, isEnd, isRotatable;
		private bool visited = false, rotate = false;
		private Vector3 prev_rotation;
		public Material normal, flowing;

		void Start()
		{
			gameObject.GetComponentInChildren<Renderer>().material = normal;
		}

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

		public void Initialise(int x, int y)
		{
			this.x = x;
			this.y = y;

			if(tag == "TileNS" || tag == "TileNES")
			{
				prev_rotation = new Vector3(0, 90, 90);
			}

			transform.Rotate(-90, -90, 90);
			transform.Rotate(90, 0, 0);
		}

		public int Y() { return this.y; }
		public int X() { return this.x; }
		public bool Visited() { return this.visited; }

		void OnMouseUp()
		{
			Debug.Log("Click");
			if(isRotatable)
			{
				rotate = true;

				if(tag == "TileNE")
				{
					transform.Rotate(0, 90, 0);
				}
				else if(tag == "TileNS" || tag == "TileNES")
				{
					Vector3 temp = transform.eulerAngles;

//					Debug.Log("prev rot:" + prev_rotation.x + " " + prev_rotation.y + " " + prev_rotation.z);
//					Debug.Log("cur rot:" + temp.x + " " + temp.y + " " + temp.z);

					int temp_x, temp_y, temp_z;
					if(temp.x == 0)
					{
						if(prev_rotation.x == 90) temp_x = 270;
						else temp_x = 90;
					}
					else temp_x = 0;

					temp_y = (int)temp.y - 90;

					if(temp.z == 0)
					{
						if(prev_rotation.z == 90) temp_z = 270;
						else temp_z = 90;
					}
					else temp_z = 0;

					transform.eulerAngles = new Vector3(temp_x, temp_y, temp_z);

//					Debug.Log("new rot:" + transform.eulerAngles.x + " " + transform.eulerAngles.y + " " + transform.eulerAngles.z);

					prev_rotation = temp;
				}


//				Debug.Log("new rot:" + transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.z);
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
			if (flow)
			{
				Renderer[] temp = gameObject.GetComponentsInChildren<Renderer>();
				foreach(Renderer r in temp)
				{
					r.material = flowing;
				}
			}
			else gameObject.GetComponentInChildren<Renderer>().material = normal;
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

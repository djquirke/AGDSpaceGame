using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PipeGame
{
	public class Tile3D : MonoBehaviour {

		private int x, y;
		public bool north, south, east, west, isStart, isEnd, isRotatable;
		private bool visited = false, rotate = false;
		private Vector3 prev_rotation;
		public Material normal, flowing;
        private float rotation_time = 0.25f;
        private Vector3 target_rotation, start_rotation, rotation_velocity = Vector3.zero;
        private List<int> target_points = new List<int>();

		void Start()
		{
            start_rotation = transform.rotation.eulerAngles;
            target_rotation = transform.rotation.eulerAngles;
            target_points.Add(0);
            target_points.Add(90);
            target_points.Add(180);
            target_points.Add(270);
		}

		void Update()
		{
            if(rotate)
            {
                Vector3 rotation = Vector3.SmoothDamp(start_rotation, target_rotation,
                                                      ref rotation_velocity, rotation_time);
                start_rotation = rotation;
                transform.rotation = Quaternion.Euler(rotation);
    
                if (isCloseTo(rotation, target_rotation))
                {
                    rotate = false;
                }
            }
            
		}

        private bool isCloseTo(Vector3 r1, Vector3 r2)
        {
            bool x = Mathf.Approximately(r1.x, r2.x);
            bool y = Mathf.Approximately(r1.y, r2.y);
            bool z = Mathf.Approximately(r1.z, r2.z);

            return x && y && z;
        }

		public void Initialise(int x, int y)
		{
			this.x = x;
			this.y = y;
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
                if (!rotate)
                {
                    start_rotation = transform.rotation.eulerAngles;
                    target_rotation.z = start_rotation.z + 90;
                }
                else
                {
                    target_rotation.z += 90;
                }

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
			Renderer rend = GetComponent<Renderer>();
			if (flow)
			{
                rend.material.SetColor("_EnergyColor", new Color(0, 0.5f, 0, 1.0f));
                rend.material.SetColor("_Color", new Color(1, 1, 1, 1.0f));
			}
			else
			{
                rend.material.SetColor("_EnergyColor", new Color(0.3f, 0.3f, 0.3f, 1.0f));
                rend.material.SetColor("_Color", new Color(1, 1, 1, 1.0f));
			}
		}

        public void FlowActive()
        {
            Renderer rend = GetComponent<Renderer>();
            if (visited) return;
            else
            {
                rend.material.SetColor("_EnergyColor", new Color(0, 0, 0, 1.0f));
                rend.material.SetColor("_Color", new Color(0.45f, 0.45f, 0.45f, 1.0f));
            }
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


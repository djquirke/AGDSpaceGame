using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

enum Direction
{
	NORTH,
	NORTHEAST,
	EAST,
	SOUTHEAST,
	SOUTH,
	SOUTHWEST,
	WEST,
	NORTHWEST
}

public class Node
{
	Node parent = null;
	GameObject obj;
	public int f, g, h;

	public Node(GameObject obj)
	{
		this.obj = obj;
	}

	public GameObject GetObject() {return obj;}
	public void SetParent(Node parent) {this.parent = parent;}
	public Node GetParent() {return parent;}

	public override bool Equals (object o)
	{
		if(o == null) return false;

		Node other = o as Node;
		if((System.Object)other == null) return false;
		return other.GetObject().transform.position.Equals(obj.transform.position);
	}

	public override int GetHashCode ()
	{
		int hash = 23;
		return hash + 31 * obj.transform.position.GetHashCode() * g.GetHashCode() * h.GetHashCode();
		//return 17 * obj.transform.position.GetHashCode() * g.GetHashCode() * h.GetHashCode();
	}
}

public class AIMovement : MonoBehaviour {

	private GameObject[] nodes;
	private List<GameObject> goal_nodes;
	private Node start_node = null;
	private Node destination;
	private List<Node> path;
	private GameObject current_standing_node;
	private GameObject current_walking_node;

	private float search_radius = 2.75f;
	private float goal_node_radius = 10f;
	private float sphere_collider_radius = 0.5f;

	//private RaycastHit hit;
	//private bool draw_line = true;
	//private List<Node> successor_nodes;
	private float lerp_step = 0;
	private int nodes_traversed = 1;
	// Use this for initialization
	private Stopwatch idle_time = new Stopwatch();
	private bool idle = false;
	private int time_to_idle = 0;
	private Stopwatch astar_run_time = new Stopwatch();
	private static int MAX_ASTAR_TIME = 500;
	private Vector3 prev_pos = new Vector3();
	private Stopwatch idle_check = new Stopwatch();
	private bool astar_failed = false;

    private bool Path_Found = false;

    private Vector3 m_CharictorTotalRotation = Vector3.zero;

    private CharacterAnimController anim = null;
    public float mesh_Roation_time = 0.1f;
    private Vector3 mesh_Rotatevelocity = Vector3.zero, LastPosItion;

	void Start()
	{
		goal_nodes = new List<GameObject>();
		//successor_nodes = new List<Node>();
		path = new List<Node>();
        anim = GetComponentInChildren<CharacterAnimController>();
		idle_check.Start();

        LastPosItion = transform.position;

	}

	private void ResetValues()
	{
		goal_nodes = new List<GameObject>();
		start_node = null;
		destination = null;
		path = new List<Node>();
		lerp_step = 0;
		nodes_traversed = 1;
		//successor_nodes.Clear();
	}

	public void Initialise () {
		nodes = GameObject.FindGameObjectsWithTag("Node");
		if(nodes.Length == 0) return;

		ResetValues();
		//start_node = new Node (node_pos);

		if(current_standing_node)
		{
			transform.position = current_standing_node.transform.position;
		}

		//get node at ai pos
		Collider[] cols = Physics.OverlapSphere(transform.position, sphere_collider_radius);
		Collider closest_node = new Collider();

		//closest_node.transform.position = new Vector3();
		//closest_node.transform.position = new Vector3(9999, 9999, 9999);//Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
		float closest_distance = Vector3.Distance(transform.position, new Vector3(9999, 9999, 9999));//closest_node.transform.position);
		foreach(Collider col in cols)
		{
			if(col.CompareTag("Node") && Vector3.Distance(col.transform.position, transform.position) < 0.1f)
			{
				start_node = new Node(col.gameObject);
				break;
			}
//			else if(col.CompareTag("Node") && Vector3.Distance(col.transform.position, transform.position) < closest_distance)
//			{
//				closest_node = col;
//				closest_distance = Vector3.Distance(col.transform.position, transform.position);
//			}
		}

		if(start_node == null)
		{
			//get closest node
			UnityEngine.Debug.LogError("start node not found!");
//			start_node = new Node(closest_node.gameObject);
			return;
		}

		current_standing_node = start_node.GetObject();
		transform.position = current_standing_node.transform.position;

		Collider[] potenial_goal_nodes = Physics.OverlapSphere(transform.position, goal_node_radius);
		foreach(Collider col in potenial_goal_nodes)
		{
			if(col.CompareTag("Node"))
			{
				NodeController nc = col.GetComponentInParent<NodeController>();
				if(nc == null) continue;
				if(nc.is_goal_node) goal_nodes.Add(col.transform.parent.gameObject);//.GetComponentInParent<Transform>().gameObject);
			}
		}

		if(goal_nodes.Count > 0)
		{
			bool running = true;
			int counter = 0;
			while (running && counter < 50)
			{
				int x = UnityEngine.Random.Range(0, goal_nodes.Count - 1);
				UnityEngine.Debug.Log("rand:" + x + "size:" + goal_nodes.Count);
				NodeController nc = goal_nodes[x].GetComponent<NodeController>();
				running = nc.getIsChosenNode();
				counter++;
				if(!running)
				{
					nc.setIsChosenNode(true);
					destination = new Node(goal_nodes[x]);
				}
			}
            Path_Found = false;

//<<<<<<< HEAD
//			RunAStar(start_node, destination);
//=======
            StartCoroutine(RunAStar(start_node, destination));
//>>>>>>> origin/master
		}
		prev_pos = transform.position;
	}
	void OnDrawGizmos() {
		if (destination != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(destination.GetObject ().transform.position, sphere_collider_radius);
		}
		if(start_node != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(start_node.GetObject().transform.position, sphere_collider_radius);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(start_node.GetObject().transform.position, goal_node_radius);
		}

	}
	// Update is called once per frame
	void Update () {
		if(idle_check.ElapsedMilliseconds > 6000)
		{
			if(Vector3.Distance(transform.position, prev_pos) < 0.25f)
			{
				//transform.position = current_standing_node.transform.position;
				Initialise();
				idle = false;
				if(idle_time.IsRunning) idle_time.Stop();
			}
			idle_check.Reset();
			idle_check.Start();
		}


		if(Path_Found && path.Count > 0 && !idle)
		{
			if(!current_walking_node || !current_standing_node) return;
			//draw path
			for(int i = 0; i < path.Count - 1; i++)
			{
				UnityEngine.Debug.DrawLine(path[i].GetObject().transform.position, path[i + 1].GetObject().transform.position, new Color(255, 0, 0));
			}

			lerp_step += Time.deltaTime;
			if(lerp_step <= 1)
			{

                Vector3 TotalDirection = current_standing_node.transform.position - current_walking_node.transform.position;

                Vector3 FaceDirection = transform.rotation * Vector3.forward;

				//rotate the model
                float angle = Mathf.Atan2(Vector3.Dot(Vector3.up,
                                      Vector3.Cross(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(m_CharictorTotalRotation), Vector3.one) * FaceDirection,
                                                    TotalDirection)),
                          Vector3.Dot(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(m_CharictorTotalRotation), Vector3.one) * FaceDirection, TotalDirection));

                Vector3 TargetAngle = m_CharictorTotalRotation + new Vector3(0, Mathf.Rad2Deg * angle, 0);


                m_CharictorTotalRotation = Vector3.SmoothDamp(m_CharictorTotalRotation,
                                                           TargetAngle,
                                                      ref mesh_Rotatevelocity, mesh_Roation_time);

                transform.rotation = Quaternion.Euler(m_CharictorTotalRotation);

                //traverse to next node
				Vector3 result = Vector3.Lerp(current_standing_node.transform.position, current_walking_node.transform.position, lerp_step);
				transform.position = result;
			}
			else
			{
				current_standing_node = current_walking_node;
				transform.position = current_walking_node.transform.position;
				prev_pos = current_walking_node.transform.position;
				lerp_step = 0;
				//check if at goal
				if(Vector3.Distance(current_walking_node.transform.position, destination.GetObject().transform.position) < 0.1f)
				{
					time_to_idle = UnityEngine.Random.Range(0, 5);
					idle_time = new Stopwatch();
					idle_time.Start();
					idle = true;
					destination.GetObject().GetComponent<NodeController>().setIsChosenNode(false);
				}
				else if(path.Count > 0)
				{
					current_walking_node = path[path.Count - nodes_traversed].GetObject();
					nodes_traversed++;
				}
				//set next node to walk to if not at goal
				//generate new path if at goal
			}


		}
		else if(idle_time.IsRunning)
		{

			if(idle_time.ElapsedMilliseconds / 1000 >= time_to_idle)
			{
				idle_time.Stop();
				Initialise();
				idle = false;
			}
		}

        if(anim)
        {
            float Distance = (transform.position - LastPosItion).magnitude;

            Distance *= Time.deltaTime;

            anim.current = Distance / anim.max;


            LastPosItion = transform.position;
        }

//		foreach(Node node in successor_nodes)
//		{
//		//if(draw_line)
//			if(node.GetParent() != null)
//			{
//				//UnityEngine.Debug.Log(node.GetObject().transform.position);
//				//UnityEngine.Debug.Log(node.GetParent().GetObject().transform.position);
//				UnityEngine.Debug.DrawLine(node.GetObject().transform.position, node.GetParent().GetObject().transform.position, new Color(255, 0, 0));
//			}
//		}
		//UnityEngine.Debug.DrawLine(nodes[1].transform.position, nodes[0].transform.position, new Color(255, 0, 0));
	}


	private List<Node> GenerateSuccessors(Node node)
	{
		List<Node> ret = new List<Node>();
		foreach(Direction dir in Enum.GetValues(typeof(Direction)))
		{
			Node successor = GenerateSuccessor(node, dir);

			if(successor != null) ret.Add(successor);
		}
		return ret;
	}

	private Node GenerateSuccessor(Node node, Direction dir)
	{
		Vector3 temp = new Vector3(node.GetObject().transform.position.x, node.GetObject().transform.position.y, node.GetObject().transform.position.z);
		int g = 0;
		//temp.x = node.transform.position.x;
		//temp.y = node.transform.position.y;
		//temp.z = node.transform.position.z;

		switch (dir) {
		case Direction.NORTH:
			temp.z += 1.5f;
			g = 15;
			break;
		case Direction.NORTHEAST:
			//return null;
			temp.z += 1.5f;
			temp.x += 1.5f;
			g = 22;
			break;
		case Direction.EAST:
			temp.x += 1.5f;
			g = 15;
			break;
		case Direction.SOUTHEAST:
//			return null;
			temp.x += 1.5f;
			temp.z -= 1.5f;
			g = 22;
			break;
		case Direction.SOUTH:
			temp.z -= 1.5f;
			g = 15;
			break;
		case Direction.SOUTHWEST:
//			return null;
			temp.x -= 1.5f;
			temp.z -= 1.5f;
			g = 22;
			break;
		case Direction.WEST:
			temp.x -= 1.5f;
			g = 15;
			break;
		case Direction.NORTHWEST:
//			return null;
			temp.x -= 1.5f;
			temp.z += 1.5f;
			g = 22;
			break;
		default:
			break;
		}

		GameObject successor = FindNode(temp);
		if (successor != null)
		{
//			Node n = new Node(successor);
//			n.g = g;
//			return n;
//			return successor;
			successor = CheckThroughWall(successor, node);
			if(successor != null)
			{
				Node n = new Node(successor);
				n.g = g;
				return n;
			}

		}
		//if(successor != null) UnityEngine.Debug.Log("successor found in direction: " + dir.ToString());
		return null;
	}

	private GameObject FindNode(Vector3 pos)
	{

		//List<GameObject> close_nodes = new List<GameObject>();
		Collider[] cols = Physics.OverlapSphere (pos, sphere_collider_radius);
		foreach (Collider col in cols)
		{
			//UnityEngine.Debug.Log ("found within radius:" + col.tag);
			if(col.CompareTag("Node"))
			{
				return col.gameObject;
				//close_nodes.Add(col.gameObject);
			}

		}

		//UnityEngine.Debug.Log(pos);
//		foreach(GameObject node in close_nodes)
//		{
//			if(Vector3.Distance(pos, node.transform.position) < 0.5f)
//			{
//				//UnityEngine.Debug.Log("node found at pos:" + node.transform.position);
//				//pos.Equals(node.transform.position))//
//				return node;
//			}
//		}
		return null;
	}

	private GameObject CheckThroughWall(GameObject node1, Node node2)
	{
		RaycastHit hit;
		if(Physics.Raycast(node1.transform.position, (node2.GetObject().transform.position - node1.transform.position), out hit, search_radius))
		{
			//UnityEngine.Debug.Log (hit.transform.tag);

			if(hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Block"))
			{
				//UnityEngine.Debug.Log("direction: " + (node2.transform.position - node1.transform.position));
				//UnityEngine.Debug.Log (hit.transform.tag + " " + hit.transform.position);
				//UnityEngine.Debug.Log("wall found between:" + node1.transform.position + node2.transform.position);
				return null;
			}
		}
		return node1;
	}

	IEnumerator RunAStar(Node start_node, Node destination)
	{
        if (start_node == null || destination == null) 
            yield break;

		bool goal_found = false;
		Node goal_node = destination;

		HashSet<Node> open_list = new HashSet<Node>();
		NPCPriorityQueue open_q = new NPCPriorityQueue();
		HashSet<Node> closed_list = new HashSet<Node>();

		open_list.Add(start_node);
		open_q.Add(start_node);
		start_node.f = 0;
		start_node.g = 0;

		astar_run_time.Start();
		while(open_list.Count > 0)
		{

            if(astar_run_time.ElapsedMilliseconds < MAX_ASTAR_TIME)
            {
		        astar_run_time.Reset();

                yield return true;
            }
			Node node = open_q.GetNext();
			open_list.Remove(node);
			if(node.Equals(destination))
			{
				goal_found = true;
				goal_node = node;
				break;
			}

			List<Node> successors = GenerateSuccessors(node);

			foreach(Node successor in successors)
			{
				//Node n2 = new Node(successor);
				successor.g += node.g;
				successor.h = EuclideanDistance(successor, goal_node);

				if(open_list.Contains(successor) || closed_list.Contains(successor)) continue;

				successor.f = successor.g + successor.h;
				successor.SetParent(node);
				open_list.Add(successor);
				open_q.Add(successor);
				//successor_nodes.Add(n2);
			}
			closed_list.Add (node);
		}

		astar_run_time.Stop();
		if(goal_found)
		{
			//traverse tree
			path.Clear ();
			TraverseTree(goal_node);
			UnityEngine.Debug.Log ("path length:" + path.Count);
            if (path.Count <= 1) 
                yield break;
			current_standing_node = path[path.Count - nodes_traversed].GetObject();
			nodes_traversed++;
			current_walking_node = path[path.Count - nodes_traversed].GetObject();
			nodes_traversed++;
			astar_failed = false;
            Path_Found = true;
		}
		else
		{
			UnityEngine.Debug.LogError("AStar not found");
			//astar_failed = true;
			//yield return new WaitForEndOfFrame();
			Initialise();
//			current_standing_node = this.gameObject;
//			nodes_traversed++;
//			current_walking_node = this.gameObject;
//			nodes_traversed++;
//			time_to_idle = UnityEngine.Random.Range(0, 5);
//			idle_time = new Stopwatch();
//			idle_time.Start();
//			idle = true;
		}
	}

	private int EuclideanDistance(Node n, Node goal)
	{
		return (int)(10 * (Mathf.Pow((goal.GetObject().transform.position.x - n.GetObject().transform.position.x), 2)
			+ Mathf.Pow((goal.GetObject().transform.position.z - n.GetObject().transform.position.z), 2)));
	}

	private void TraverseTree(Node node)
	{
		path.Add(node);
		if(node.GetParent() == null) return;
		TraverseTree(node.GetParent());
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Door"))
		{
			other.GetComponent<DoorManager>().Open();
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag.Equals("Door"))
		{
			other.GetComponent<DoorManager>().Close();
		}
	}
}

















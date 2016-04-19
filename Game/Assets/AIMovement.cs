using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

	private float search_radius = 2.5f;

	//private RaycastHit hit;
	//private bool draw_line = true;
	private List<Node> successor_nodes;
	// Use this for initialization

	void Start()
	{
		goal_nodes = new List<GameObject>();
		successor_nodes = new List<Node>();
		path = new List<Node>();
	}

	public void Initialise () {
		nodes = GameObject.FindGameObjectsWithTag("Node");
		if(nodes.Length == 0) return;

		//start_node = new Node (node_pos);

		//find all goal nodes
		foreach(GameObject node in nodes)
		{
			NodeController nc = node.GetComponent<NodeController>();
			if(nc == null) continue;
			if(nc.is_goal_node) goal_nodes.Add(node);
			if(Vector3.Distance(transform.position, node.transform.position) < 0.5f)
			{
				start_node = new Node(node); 
				//Debug.Log("Start node found! pos:" + node.transform.position);
			}
		}

		//Debug.Log("goal nodes found:" + goal_nodes.Count);

		bool running = true;
		while (running)
		{
			int x = UnityEngine.Random.Range(0, goal_nodes.Count);
			NodeController nc = goal_nodes[x].GetComponent<NodeController>();
			running = nc.getIsChosenNode();
			if(!running)
			{
				nc.setIsChosenNode(true);
				destination = new Node(goal_nodes[x]);
			}
		}

		Collider[] cols = Physics.OverlapSphere (start_node.GetObject ().transform.position, 2.5f);
		foreach (Collider col in cols)
		{
			Debug.Log ("found within radius:" + col.tag);
		}


		RunAStar(start_node, destination);

//		foreach (GameObject node in nodes)
//		{
//			Node n = new Node(node);
//			//Debug.Log ("Starting node pos:" + node.transform.position);
//			successor_nodes.Add (n);
//			List<GameObject> successors = GenerateSuccessors(node);
//
//			foreach(GameObject successor in successors)
//			{
//				Node n2 = new Node(successor);
//				n2.SetParent(n);
//				successor_nodes.Add(n2);
//			}
//		}
		//Debug.Log(successor_nodes.Count);
	}
	void OnDrawGizmos() {
		if (start_node != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(start_node.GetObject ().transform.position, 2.5f);
		}
	}
	// Update is called once per frame
	void Update () {
		if(path.Count > 0)
		{
			//draw path
			for(int i = 0; i < path.Count - 1; i++)
			{
				Debug.DrawLine(path[i].GetObject().transform.position, path[i + 1].GetObject().transform.position, new Color(255, 0, 0));
			}
		}
//		foreach(Node node in successor_nodes)
//		{
//		//if(draw_line)
//			if(node.GetParent() != null)
//			{
//				//Debug.Log(node.GetObject().transform.position);
//				//Debug.Log(node.GetParent().GetObject().transform.position);
//				Debug.DrawLine(node.GetObject().transform.position, node.GetParent().GetObject().transform.position, new Color(255, 0, 0));
//			}
//		}
		//Debug.DrawLine(nodes[1].transform.position, nodes[0].transform.position, new Color(255, 0, 0));
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
		//if(successor != null) Debug.Log("successor found in direction: " + dir.ToString());
		return null;
	}

	private GameObject FindNode(Vector3 pos)
	{
		//Debug.Log(pos);
		foreach(GameObject node in nodes)
		{
			if(Vector3.Distance(pos, node.transform.position) < 0.5f)
			{
				//Debug.Log("node found at pos:" + node.transform.position);
				//pos.Equals(node.transform.position))//
				return node;
			}
		}
		return null;
	}

	private GameObject CheckThroughWall(GameObject node1, Node node2)
	{
		RaycastHit hit;
		if(Physics.Raycast(node1.transform.position, (node2.GetObject().transform.position - node1.transform.position), out hit, search_radius))
		{
			//Debug.Log (hit.transform.tag);

			if(hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Block"))
			{
				//Debug.Log("direction: " + (node2.transform.position - node1.transform.position));
				//Debug.Log (hit.transform.tag + " " + hit.transform.position);
				//Debug.Log("wall found between:" + node1.transform.position + node2.transform.position);
				return null;
			}
		}
		return node1;
	}

	private void RunAStar(Node start_node, Node destination)
	{
		if(start_node == null || destination == null) return;

		bool goal_found = false;
		Node goal_node = destination;

		HashSet<Node> open_list = new HashSet<Node>();
		NPCPriorityQueue open_q = new NPCPriorityQueue();
		HashSet<Node> closed_list = new HashSet<Node>();

		open_list.Add(start_node);
		open_q.Add(start_node);
		start_node.f = 0;
		start_node.g = 0;

		while(open_list.Count > 0)
		{
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

		if(goal_found)
		{
			//traverse tree
			path.Clear ();
			TraverseTree(goal_node);
			Debug.Log ("path length:" + path.Count);
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

















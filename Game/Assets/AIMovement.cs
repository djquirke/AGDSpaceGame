using UnityEngine;
using System.Collections;

public class AIMovement : MonoBehaviour {

	private GameObject[] nodes;
	private RaycastHit hit;
	private bool draw_line = true;
	// Use this for initialization
	void Start () {
		nodes = GameObject.FindGameObjectsWithTag("Node");

	}
	
	// Update is called once per frame
	void Update () {
		if(Physics.Raycast(nodes[0].transform.position, (nodes[1].transform.position - nodes[0].transform.position), out hit))
		{
			Debug.Log(hit.transform.tag);
			if(hit.transform.CompareTag("Wall"))
			{
				Debug.Log("wall hit, invalid successor");
				draw_line = false;
			}
		}
		if(draw_line)
			Debug.DrawLine(nodes[1].transform.position, nodes[0].transform.position, new Color(255, 0, 0));

		//Debug.DrawLine(nodes[1].transform.position, nodes[0].transform.position, new Color(255, 0, 0));
	}
}

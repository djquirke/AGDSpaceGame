using UnityEngine;
using System.Collections;

public class NodeController : MonoBehaviour {

	public bool is_goal_node = true;
	[SerializeField] bool is_chosen_node = false;
	[SerializeField] bool is_start_node = false;
	public Vector3 pos;

	public bool getIsChosenNode() {return is_chosen_node;}
	public void setIsChosenNode(bool b) {is_chosen_node = b;}
	public bool getIsStartNode() {return is_start_node;}
	public void setIsStartNode(bool b) {is_start_node = b;}

	// Use this for initialization
	void Start () {
		pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

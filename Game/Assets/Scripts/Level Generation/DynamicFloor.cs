using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicFloor : MonoBehaviour {
    private GameObject[] iList;
    private GameObject[] fNodes;
	private GameObject[] doors;
	private List<GameObject> walls;
    private Renderer rend;
	private float distance_threshold = 0.5f;
    public int type = 0;
    public Material[] MaterialList;
	// Use this for initialization
	public void Calculate() {
		//yield return new WaitForSeconds(3);
		rend = GetComponent<Renderer>();
		
		fNodes = GameObject.FindGameObjectsWithTag("DynamicFloor");
		doors = GameObject.FindGameObjectsWithTag("Door");
		Transform[] tforms = gameObject.GetComponentsInChildren<Transform>();
		walls = new List<GameObject>();
		foreach(Transform tform in tforms)
		{
			if (tform.CompareTag("Wall"))
			{
				walls.Add(tform.gameObject);
			}
		}
		Debug.Log (walls.Count);
		
		iList = new GameObject[fNodes.Length + doors.Length];
		for (int i = 0; i < fNodes.Length; i += 1)
		{
			iList[i] = fNodes[i];
		}
		for (int j = 0; j < doors.Length; j += 1)
		{
			iList[fNodes.Length + j] = doors[j];
		}
		fNodes = iList;
		Transform[] tformss = gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform child in tformss)
		{
			//Debug.Log(child);
			//if(child.gameObject.GetComponentInChildren<Transform>().CompareTag("Wall")) Debug.Log("yay");
			if(child.CompareTag("Door"))
			{
				if (hasAdjacent(child.gameObject, fNodes) || hadAdjacentDoor(child.gameObject, fNodes))
				{
					type += 1;
				}
			}

		}
		applyMaterial();
		adjustOrientation();
	}
	void Start () {
		//StartCoroutine(Calculate());
        
	}
   
    public void adjustOrientation()
    {
        GameObject north = transform.Find("North").gameObject;
        GameObject east = transform.Find("East").gameObject;
        GameObject south = transform.Find("South").gameObject;
        GameObject west = transform.Find("West").gameObject;

		//List<Transform> wall_tforms = new List<Transform>();
		//Transform[] temp = north.GetComponentsInChildren<Transform>();
		GameObject north_wall = north.transform.FindChild("Wall").gameObject;
		north_wall.transform.parent = null;
		GameObject east_wall = east.transform.FindChild("Wall").gameObject;
		east_wall.transform.parent = null;
		GameObject south_wall = south.transform.FindChild("Wall").gameObject;
		south_wall.transform.parent = null;
		GameObject west_wall = west.transform.FindChild("Wall").gameObject;
		west_wall.transform.parent = null;

		switch (type) {
		case 1:
			while(!hasAdjacent(north, fNodes))
			{

				transform.Rotate(0, 0, 90);
			}
			break;
		case 2:
			if (hasAdjacent(north, fNodes) && hasAdjacent(south, fNodes) || hasAdjacent(east, fNodes) && hasAdjacent(west, fNodes))
			{
				if (hasAdjacent(north, fNodes))
				{
					transform.Rotate(0, 0, 90);
				}
			}
			else
			{
				while(!(hasAdjacent(north, fNodes)) || !(hasAdjacent(east, fNodes)))
				{
					transform.Rotate(0, 0, 90);
				}
			}
			break;
		case 3:
			while(hasAdjacent(south, fNodes))
			{
				transform.Rotate(0, 0, 90);
			}
			break;
		default:
			break;
		}
		north_wall.transform.parent = north.transform;
		east_wall.transform.parent = east.transform;
		south_wall.transform.parent = south.transform;
		west_wall.transform.parent = west.transform;
    }

    public void applyMaterial()
    {
        GameObject north = transform.Find("North").gameObject;
        GameObject east = transform.Find("East").gameObject;
        GameObject south = transform.Find("South").gameObject;
        GameObject west = transform.Find("West").gameObject;

		switch (type) {
		case 1:
			rend.material = MaterialList[0];
			break;
		case 2:
			if (hasAdjacent(north, fNodes) && hasAdjacent(south, fNodes) || hasAdjacent(east, fNodes) && hasAdjacent(west, fNodes)) rend.material = MaterialList[2];
			else rend.material = MaterialList[1];
			break;
		case 3:
			rend.material = MaterialList[3];
			break;
		case 4:
			rend.material = MaterialList[4];
			break;
		default:
			break;
		}
    }
    public bool hadAdjacentDoor(GameObject cNode, GameObject[] eNode)
    {
        return false;
    }
    public bool hasAdjacent(GameObject cNode, GameObject[] eNode){
        bool foundNode = false;
        foreach(GameObject node in eNode){
            foreach (Transform child in node.transform)
            {
                if (cNode.transform != child.transform)
                {
					if (Vector3.Distance(cNode.transform.position, child.transform.position) < distance_threshold)
                    {
                    	foundNode = true;
						Transform[] temp = cNode.GetComponentsInChildren<Transform>();
						foreach(Transform tem in temp)
						{
							if(tem.CompareTag("Wall"))
							{
								Destroy(tem.gameObject);
							}
						}
						//Debug.Log (temp.CompareTag("Wall"));
						//Destroy(temp.gameObject);
//						foreach(GameObject wall in walls)
//						{
//							Debug.Log(Vector3.Distance(wall.transform.position, cNode.transform.position));
//							if(Vector3.Distance(wall.transform.position, child.transform.position) < distance_threshold)
//							{
//								Destroy(wall);
//							}
//						}
                    }
                }
            }  
        
        }
        return foundNode;
    }
}

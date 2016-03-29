using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicFloor : MonoBehaviour {
    private GameObject[] iList;
    private GameObject[] fNodes;
    private GameObject[] doors;
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
		foreach (Transform child in transform)
		{
			if (hasAdjacent(child.gameObject, fNodes) || hadAdjacentDoor(child.gameObject, fNodes))
			{
				type += 1;
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

                    }
                }
            }  
        
        }
        return foundNode;
    }
}

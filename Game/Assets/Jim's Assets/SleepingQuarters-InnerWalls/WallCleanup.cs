using UnityEngine;
using System.Collections;

public class WallCleanup : MonoBehaviour {
    public GameObject[] gList;
	// Use this for initialization
	void Start () {
	    gList = GameObject.FindGameObjectsWithTag("Wall");
        for (var i = 0; i < gList.Length; i += 1){
            hasOther(gList, gList[i]);
        }
	}
	public bool hasOther(GameObject[] List, GameObject g){
        bool isFound = false;
        for (var i = 0; i < List.Length - 1; i += 1)
        {
            if (List[i] == g)
            {
                Debug.Log("asd");
            }
        }
        return isFound;
    }
	// Update is called once per frame
	void Update () {
	
	}
}

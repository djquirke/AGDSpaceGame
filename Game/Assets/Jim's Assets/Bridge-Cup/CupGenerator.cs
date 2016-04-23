using UnityEngine;
using System.Collections;

public class CupGenerator : MonoBehaviour {
    public Material[] mats;
	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material = mats[Random.Range(0,mats.Length)];
	}
	
}

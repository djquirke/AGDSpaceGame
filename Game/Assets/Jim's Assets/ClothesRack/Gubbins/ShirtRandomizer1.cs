using UnityEngine;
using System.Collections;

public class ShirtRandomizer1 : MonoBehaviour {
    public Material[] mats;
	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material = mats[Random.Range(0, mats.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

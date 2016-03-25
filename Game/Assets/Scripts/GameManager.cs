using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		GameObject.FindGameObjectWithTag("LoadManager").GetComponent<LoadManager>().gManagerReady();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

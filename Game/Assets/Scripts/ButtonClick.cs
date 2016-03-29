using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour {

	private MissionManager mission_manager;

	// Use this for initialization
	void Start () {

        EventSystem.current.SetSelectedGameObject(gameObject);

		mission_manager = GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>();

		this.GetComponent<Button>().onClick.AddListener(() => LoadGame());
      
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadGame()
	{
		Debug.Log("Click");
		mission_manager.StartMission();
	}
}

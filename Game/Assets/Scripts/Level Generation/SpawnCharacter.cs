using UnityEngine;
using System.Collections;

public class SpawnCharacter : MonoBehaviour {

	public GameObject medic, engineer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnPlayer(MissionType type)
	{
		GameObject temp;
		switch (type)
		{
		case MissionType.ENGINEERING:
			temp = (GameObject)Instantiate(engineer);
			temp.transform.position = this.transform.position;
			break;
		case MissionType.OXYGEN:
			temp = (GameObject)Instantiate(engineer);
			temp.transform.position = this.transform.position;
			break;
		case MissionType.ILLNESS:
			temp = (GameObject)Instantiate(medic);
			temp.transform.position = this.transform.position;
			break;
		}
	}
}

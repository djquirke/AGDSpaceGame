using UnityEngine;
using System.Collections;

public class OpenSesame : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag);
		if(other.tag.Equals("Door"))
		{
			other.GetComponent<DoorManager>().Open();
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag.Equals("Door"))
		{
			StartCoroutine(other.GetComponent<DoorManager>().Close());
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.tag.Equals("Door"))
		{
			other.GetComponent<DoorManager>().StayOpen();
		}
	}
}

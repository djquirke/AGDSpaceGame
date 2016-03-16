using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

	private bool open;

	// Use this for initialization
    private bool doorState = false;
	void Start () {
        //StartCoroutine(Tick());
		Animator[] doors = GetComponentsInChildren<Animator>();
		foreach(Animator door in doors)
		{
			door.Play("Close", -1, 0);
		}
		open = false;
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log(other.collider.GetType().Equals(typeof(BoxCollider)));
		Debug.Log(other.GetType() == (typeof(BoxCollider)));
		if(other.tag.Equals("Player") || other.tag.Equals("AI"))// && other.collider.GetType().Equals(typeof(BoxCollider)))
		{
			Animator[] doors = GetComponentsInChildren<Animator>();
			foreach(Animator door in doors)
			{
				door.Play("Open", -1, 0);
			}
			open = true;
		}
	}

	void OnTriggerExit()
	{
		if(open)
		{
			Animator[] doors = GetComponentsInChildren<Animator>();
			foreach(Animator door in doors)
			{
				door.Play("Close", -1, 0);
			}
			open = false;
		}
	}
}

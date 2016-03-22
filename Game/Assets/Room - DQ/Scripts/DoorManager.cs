using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

	private bool open;
	[SerializeField] bool gives_access = false;

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
		Debug.Log(other.GetType());
		//Debug.Log(other.GetType() == (typeof(BoxCollider)));
		if(other.tag.Equals("Player") || other.tag.Equals("AI"))// && other.collider.GetType().Equals(typeof(BoxCollider)))
		{
			//Open();
		}
	}

	void OnTriggerExit()
	{
		if(open)
		{
			//Close();
		}
	}

	public void SetAccess(bool b) {gives_access = b;}
	public bool GetAccess() {return gives_access;}

	public void Open()
	{
		Animator[] doors = GetComponentsInChildren<Animator>();
		foreach(Animator door in doors)
		{
			door.Play("Open", -1, 0);
		}
		open = true;
	}

	public void Close()
	{
		Animator[] doors = GetComponentsInChildren<Animator>();
		foreach(Animator door in doors)
		{
			door.Play("Close", -1, 0);
		}
		open = false;
	}
}

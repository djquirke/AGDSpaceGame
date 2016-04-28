using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

	//private bool open;
	public bool gives_access = false;
	private Animator[] doors;
	private bool open;

	// Use this for initialization
    private bool doorState = false;
	void Start () {
		doors = GetComponentsInChildren<Animator>();
		foreach(Animator door in doors)
		{
			door.Play("Close", -1, 0);
		}
		//open = false;
	}

	public void SetAccess(bool b) {gives_access = b;}
	public bool GetAccess() {return gives_access;}

	public void Open()
	{
		if (doors == null || open)
			return;

		foreach(Animator door in doors)
		{
			AnimateDoor(door, "Open");
		}
		open = true;
	}

	public void StayOpen() {open = true;}

	public IEnumerator Close()
	{
		open = false;
		yield return new WaitForSeconds(0.25f);
		if(open) yield break;
		foreach(Animator door in doors)
		{
			AnimateDoor(door, "Close");
		}
	}

	private void AnimateDoor(Animator door, string anim_name)
	{
		float norm_t = door.GetCurrentAnimatorStateInfo(0).normalizedTime;
		float t;
		if(norm_t >= 1)
			t = 0;
		else
			t = 1 - norm_t;
		door.Play(anim_name, -1, t);
	}
}

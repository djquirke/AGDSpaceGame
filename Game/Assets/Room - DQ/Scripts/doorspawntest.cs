using UnityEngine;
using System.Collections;

public class doorspawntest : MonoBehaviour {

	public GameObject door;

	// Use this for initialization
	void Start () {
		Transform[] children = GetComponentsInChildren<Transform>();
		foreach(Transform child in children)
		{
			if(child.tag.Equals("Door Spawn"))
			{
				//Renderer[] c2 = child.GetComponentsInChildren<Renderer>();
				Transform[] c2 = child.GetComponentsInChildren<Transform>();
				foreach(Transform c in c2)
				{
					//Debug.Log(c.tag);
					if(c.tag.Equals("Door"))
					{
						//c.collider.enabled = false;
					}
						//Destroy(c.gameObject);
						//c.renderer.enabled = false;
				}
//				foreach(Renderer c in c2)
//				{
//					Debug.Log(c.tag);
//					if(c.tag.Equals("Door"))
//					{
//						Destroy(c.gameObject);
//					   	//c.enabled = false;
//						Debug.Log("enable door rendering");
//					}
//					else
//						c.enabled = true;
//				}
				//GameObject newdoor = (GameObject)Instantiate(door);//, child.localPosition, child.localRotation);
				//newdoor.transform.SetParent(transform);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

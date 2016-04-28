using UnityEngine;
using System.Collections;

public class SpawnPrefab : MonoBehaviour {

	public GameObject prefab;

	void Awake()
	{
		if(prefab)
		{
			GameObject obj = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
			//Debug.Log(transform.localScale.x);
			obj.transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
			obj.transform.SetParent(transform.parent);
			DestroyImmediate(transform.gameObject);
		}
	}
}

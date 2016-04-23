using UnityEngine;
using System.Collections;

public class SpawnPrefab : MonoBehaviour {

	public GameObject prefab;

	void Awake()
	{
		if(prefab)
		{
			GameObject obj = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
			obj.transform.SetParent(transform.parent);
			Destroy(transform.gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomSplitter : MonoBehaviour
{
    public GameObject[] roomsets;
    public int[] weights;
	private bool split = false;

    List<int> weightsList = new List<int>();

	void Start()
	{
		Split ();
	}

    public void Split()
    {
		if(!split)
		{
			for (int i = 0; i < weights.Length; i++)
			{
				AddWeight(weights[i], i);
			}
			
			
			int rand = Random.Range(0, weightsList.Count);
			
			if (roomsets[weightsList[rand]] == null)
			{
				return;
			}
			else
			{
				GameObject rooms = (GameObject)Instantiate(roomsets[weightsList[rand]], transform.position, transform.rotation);
			}

			split = true;
			Destroy(gameObject);
		}
        
    }

    void AddWeight(int weight, int value)
    {
        for (int i = 0; i < weight; i++)
        {
            weightsList.Add(value);
        }
    }
}

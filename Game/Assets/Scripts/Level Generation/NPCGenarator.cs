using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCGenarator : MonoBehaviour {

    public GameObject[] NPCsets;
    public int[] weights;
    public int NumberOfNPCs = 0;
	private RoomSize r_size;
	private RoomType r_type;

    List<int> weightsList = new List<int>();

	public void Initialise(MissionType mission_type)
	{
		r_type = gameObject.GetComponent<RoomManager>().type;
		r_size = gameObject.GetComponent<RoomManager>().size;

		NumberOfNPCs = Random.Range (0, 2 * ((int)r_size + 1));
		Debug.Log (r_size + " " + NumberOfNPCs);
		if(NumberOfNPCs == 0) return;

		if (mission_type != MissionType.ILLNESS) {
			weights = new int[]{1};
		}
		else
		{
			weights = new int[]{1, 2};
		}

		while (0 < NumberOfNPCs--)
		{
			Generate();
		}
	}

    void Generate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            AddWeight(weights[i], i);
        }


        int rand = Random.Range(0, weightsList.Count);

        if (NPCsets[weightsList[rand]] == null)
        {
            return;
        }
        else
        {
            GameObject rooms = (GameObject)Instantiate(NPCsets[weightsList[rand]], transform.position + new Vector3(0f,3f,0), Quaternion.Euler(0.0f, 0.0f, 0.0f));
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

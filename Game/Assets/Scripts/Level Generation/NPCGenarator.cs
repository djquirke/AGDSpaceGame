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

		NumberOfNPCs = Random.Range (0, ((int)r_size + 2));
//		NumberOfNPCs = Random.Range(0, 2);
		//Debug.Log (r_size + " " + NumberOfNPCs);
		if(NumberOfNPCs == 0) return;
//		if(r_type != RoomType.HALLWAY) return;
//		weights = new int[]{10, 5};
		if (mission_type != MissionType.ILLNESS || r_type == RoomType.FLIGHT_DECK) {
			weights = new int[]{1};
		}
		else
		{
			if(r_type != RoomType.MEDIC)
				weights = new int[]{10, 5};
			else
				weights = new int[]{0, 15};
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
			//choose a random node to spawn at
			NodeController[] nodes = gameObject.GetComponentsInChildren<NodeController>();
			if(nodes.Length == 0)
			{
				Debug.Log("no nodes avail");
				return;
			}
			bool running = true;
			while(running)
			{
				int r = Random.Range(0, nodes.Length);
				running = nodes[r].getIsStartNode();
				if(!running)
				{
					Instantiate(NPCsets[weightsList[rand]], nodes[r].gameObject.GetComponent<Transform>().position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
					//obj.GetComponent<AIMovement>().Initialise(nodes[r].gameObject);
					nodes[r].setIsStartNode(true);
				}
			}



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

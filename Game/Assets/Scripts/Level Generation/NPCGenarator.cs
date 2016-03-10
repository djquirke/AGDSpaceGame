using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCGenarator : MonoBehaviour {

    public GameObject[] NPCsets;
    public int[] weights;
    public int NumberOfNPCs = 0;

    List<int> weightsList = new List<int>();

    // Use this for initialization

    void Start()
    {
        while (0 != NumberOfNPCs--)
            Genarate();
    }

    void Genarate()
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

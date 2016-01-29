using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomSplitter : MonoBehaviour
{
    public GameObject[] roomsets;
    public int[] weights;

    List<int> weightsList = new List<int>();

    // Use this for initialization
    void Start()
    {
        Split();
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Split()
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
            GameObject rooms = (GameObject)Instantiate(roomsets[weightsList[rand]], transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        }


        Destroy(gameObject);
    }

    void AddWeight(int weight, int value)
    {
        for (int i = 0; i < weight; i++)
        {
            weightsList.Add(value);
        }
    }
}

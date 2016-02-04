using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OxygenManager : MonoBehaviour
//{

//    public List<GameObject> neighbours;
//    List<Connection> connections = new List<Connection>();
//    public float roomDistanceMax;
//    public int leaks = 0;
//    public float oxygen = 0.0f;
//    public float maxOxygen = 100.0f;

//    protected struct Connection
//    {
//        public bool open = false;
//        public float flowRate = 0.1f;
//        public GameObject neighbour;
//    }

//    // Use this for initialization
//    void Start()
//    {
//        FindNeighbours();
//        GenerateConnections();
//    }

//    private void GenerateConnections()
//    {
//        foreach (var neighbour in neighbours)
//        {
//            Connection tempCon = new Connection();
//            tempCon.neighbour = neighbour;
//            connections.Add(tempCon);
//        }
//    }

//    private void FindNeighbours()
//    {
//        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
//        foreach (var room in rooms)
//        {
//            if (Vector3.Distance(this.transform.position, room.transform.position) < roomDistanceMax)
//            {
//                neighbours.Add(room);
//            }
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        foreach (var connection in connections)
//        {
//            if (connection.open)
//            {
//                if (connection.neighbour.GetComponent<OxygenManager>().oxygen < oxygen)
//                {
//                    AddOxygen(-connection.flowRate);
//                    connection.neighbour.GetComponent<OxygenManager>().AddOxygen(connection.flowRate);
//                }
//                else
//                {
//                    AddOxygen(connection.flowRate);
//                    connection.neighbour.GetComponent<OxygenManager>().AddOxygen(-connection.flowRate);
//                }
//            }
//        }
//        AddOxygen(leaks * -1.0f);
//    }
//}

{
    
    public GameObject airBarPrefab;
    public GameObject roomTile;
    GameObject airBar;
    //float oxygen = 0;
    //float co2 = 0;
    public float air = 0;
    float timePassed = 0.0f;
    float newAir;
    List<GameObject> neighbours = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
            //airBar = (GameObject)Instantiate(airBarPrefab, this.transform.position, this.transform.rotation);
            //airBar.transform.parent = transform;
            InvokeRepeating("AirUpdate", Random.Range(0.2f, 2.0f), 2.5f);
    }
    public void UpdateNeighbours()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("floor");
        neighbours = new List<GameObject>();
        foreach (GameObject tile in tiles)
        {
            if (Vector3.Distance(this.transform.position, tile.transform.position) <= 1.1f &&
                Vector3.Distance(this.transform.position, tile.transform.position) > 0.1f)
            {
                neighbours.Add(tile);
            }
        }
        neighbours.Add(this.gameObject);
    }
    void AirUpdate()
    {
        float airTotal = 0.0f;
            for (int i = 0; i < neighbours.Count; i++)
            {
                    airTotal += neighbours[i].GetComponent<OxygenManager>().air;
            }
            float airEach = airTotal / neighbours.Count;
            airEach = Mathf.Min(airEach, 1.0f);

            for (int i = 0; i < neighbours.Count; i++)
            {
                neighbours[i].GetComponent<OxygenManager>().air = airEach;
                air = airEach;
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (air != 0.0f)
        {
            air = Mathf.Clamp(air, 0.0f, 1.0f);
        //    airBar.GetComponent<oxycolour>().o2Level = air;
        //    airBar.transform.localScale = new Vector3(1.0f, Mathf.Round(air * 10.0f), 1.0f);
        //    airBar.transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Round((air / 2) * 10.0f) / 10.0f) , transform.position.z);
        }
    }

    public void AddOxygen(float airToAdd)
    {
        air += airToAdd;
    }

    public bool OxygenStarvation()
    {
        return (air < 0.15f);
    }
}
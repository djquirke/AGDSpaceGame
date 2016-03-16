//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class RoomManager : MonoBehaviour
//{
//
//    public bool safe = false;
//    public bool wideRoom = false;
//    [SerializeField] List<GameObject> neighbours = new List<GameObject>();
//    public List<GameObject> doors;
//    // Use this for initialization
//    void Start()
//    {
//		doors = new List<GameObject> ();
//		foreach (Transform child in transform.GetChild(0).transform) {
//			if(child.tag == "Door")
//			{
//				doors.Add(child.gameObject);
//			}
//		}
//		foreach (Transform child in transform) {
//			if(child.tag == "Door")
//			{
//				doors.Add(child.gameObject);
//			}
//		}
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//
//    }
//
//    public void Rotate()
//    {
//        if(wideRoom)
//        {
//            transform.Rotate(new Vector3(0, 180, 0));
//        }
//        else
//        {
//            transform.Rotate(new Vector3(0, 90, 0));
//        }
//    }
//
//    void GetNeighbours()
//    {
//        for (int i = 0; i < doors.Count; i++)
//        {
//            GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door");
//            for (int j = 0; j < allDoors.Length; j++)
//            {
//                if (doors[i] != allDoors[j] && Vector3.Distance(doors[i].transform.position, allDoors[j].transform.position) < 1.0f)
//                {
//                    if(allDoors[j].transform.parent.parent.gameObject.GetComponent<RoomManager>())
//						neighbours.Add(allDoors[j].transform.parent.parent.gameObject);
//                    else if (allDoors[j].transform.parent.parent.gameObject.GetComponent<RoomManager>())
//                        neighbours.Add(allDoors[j].transform.parent.parent.gameObject);
//				}
//				if (doors[i] != allDoors[j] && Vector3.Distance(doors[i].transform.position, allDoors[j].transform.position) < 1.0f)
//				{
//					if(allDoors[j].transform.parent.gameObject.GetComponent<RoomManager>())
//						neighbours.Add(allDoors[j].transform.parent.gameObject);
//					else if (allDoors[j].transform.parent.gameObject.GetComponent<RoomManager>())
//						neighbours.Add(allDoors[j].transform.parent.gameObject);
//				}
//            }
//        }
//    }
//
//    public void SafeNeighbourCheck()
//    {
//        GetNeighbours();
//        for (int i = 0; i < neighbours.Count; i++)
//        {
//            if (neighbours[i].GetComponent<RoomManager>().safe)
//                safe = true;
//        }
//    }
//}

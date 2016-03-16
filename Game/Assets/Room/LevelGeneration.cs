//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class LevelGeneration : MonoBehaviour {
//
//    enum State
//    {
//        split,
//        check,
//        complete
//    }
//
//    State state = State.split;
//
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//        switch (state)
//        {
//            case State.split:
//                GameObject[] roomTemplates = GameObject.FindGameObjectsWithTag("RoomTemplate");
//                if (roomTemplates.Length > 0)
//                {
//                    for (int i = 0; i < roomTemplates.Length; i++)
//                    {
//                        roomTemplates[i].GetComponent<RoomSplitter>().Split();
//                    }
//                }
//                else
//                    state = State.check;
//                break;
//            case State.check:
//                int maxCount = 5;
//                int count = 0;
//                while (count < maxCount)
//                {
//                    GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
//                    for (int j = 0; j < 3; j++)
//                    {
//                        for (int i = 0; i < rooms.Length; i++)
//                        {
//                            rooms[i].GetComponent<RoomManager>().SafeNeighbourCheck();
//                        }
//                    }
//                    bool unfinished = false;
//                    for (int i = 0; i < rooms.Length; i++)
//				{
//					rooms[i].GetComponent<RoomManager>().SafeNeighbourCheck();
//					if (rooms[i].GetComponent<RoomManager>().safe == false)
//                        {
//                            rooms[i].GetComponent<RoomManager>().Rotate();
//                            unfinished = true;
//                        }
//                    }
//                    if (unfinished == false)
//                    {
//                        break;
//                    }
//                    count++;
//                }
//                state = State.complete;
//                break;
//            case State.complete:
//                break;
//            default:
//                break;
//        }
//	}
//}

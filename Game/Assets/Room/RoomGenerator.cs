//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class RoomGenerator : MonoBehaviour
//{
//    public int width = 5;
//    public int height = 5;
//    public GameObject WallPrefab;
//
//    // Use this for initialization
//    void Start() {
//
//
//
//        List<List<GameObject>> walls = new List<List<GameObject>>();
//        for (int i = 0; i < width; i++)
//        {
//            List<GameObject> wallsInner = new List<GameObject>();
//            for (int j = 0; j < height; j++)
//            {
//                wallsInner.Add((GameObject)Instantiate(WallPrefab, this.transform.position, this.transform.rotation));
//            }
//            walls.Add(wallsInner);
//        }
//
//        for (int x = 0; x < width; x++)
//        {
//            for (int y = 0; y < height; y++)
//            {
//                Vector3 newPos = this.transform.position + new Vector3(x + 0.5f - width / 2.0f, 1.0f, y + 0.5f - height / 2.0f);
//                walls[x][y].transform.position = newPos;
//                walls[x][y].transform.parent = this.transform;
//            }
//        }
//
//        for(int x = 1; x < width - 1; x++)
//        {
//            for(int y = 1; y < height - 1; y++)
//            {
//                walls[x][y].gameObject.SetActive(false);
//            }
//        }
//	}
//
//    // Update is called once per frame
//    void Update()
//    {
//
//    }
//}

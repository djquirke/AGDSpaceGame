using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

    static bool m_instanceCreated = false;

	// Use this for initialization
	void Start () {
        if(!m_instanceCreated)
        {
            //DontDestroyOnLoad(gameObject);
            m_instanceCreated = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SelectLevel(string level)
    {
        Application.LoadLevel(level);
    }

    public void test()
    {
        Debug.Log("Button Click");

    }
}

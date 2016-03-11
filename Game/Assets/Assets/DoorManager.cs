using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

	// Use this for initialization
    private bool doorState = false;
	void Start () {
        StartCoroutine(Tick());
	}
    IEnumerator Tick()
    {
        yield return new WaitForSeconds(1);
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in player)
        {
            float dist = Vector3.Distance(p.transform.position, transform.position);
            if (dist < 3 && doorState == false)
            {
                doorState = true;
                triggerAnim(true);
            }
            else if (dist > 3 && doorState == true)
            {
                doorState = false;
                triggerAnim(false);
            }
        }
        StartCoroutine(Tick());
        
    }
    public void triggerAnim(bool state)
    {
        if (state == true)
        {
            GetComponent<Animator>().Play("Open", -1, 0f);
        }
        else
        {
            GetComponent<Animator>().Play("Close", -1, 0f);
        }
    }
	void Update () {
	
	}
}

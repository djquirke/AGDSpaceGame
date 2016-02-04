using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public bool activated = false;
    public GameObject owner = null;
    public GameObject targetTele = null;
    
    // Use this for initialization
    void Start()
    {
        owner = GameObject.FindGameObjectWithTag("Player");
        foreach (var tele in GameObject.FindGameObjectsWithTag("Teleporter"))
        {
            if (tele.GetComponent<Teleporter>().owner == owner)
            {
                if(tele.transform != this.transform)
                {
                    targetTele = tele;
                    tele.GetComponent<Teleporter>().targetTele = gameObject;
                }
            }
        }
    }

    public void TriggerEffects()
    {
        //FLASHY STUFF
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Player")
            return;
        if(targetTele != null)
        {
            targetTele.GetComponent<Teleporter>().activated = false;
            col.transform.position = targetTele.transform.position;
            col.transform.Translate(new Vector3(0.0f, 1.0f, 0.0f));
            TriggerEffects();
            targetTele.GetComponent<Teleporter>().TriggerEffects();
        }
    }

    void OnCollisionExit(Collision col)
    {
        activated = true;
    }
}

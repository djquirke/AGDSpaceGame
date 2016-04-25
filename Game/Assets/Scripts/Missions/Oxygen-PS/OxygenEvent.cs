using UnityEngine;
using System.Collections;

public class OxygenEvent : Event {

    public float Start_Raduis_Min = 0, Start_Raduis_Max = 0, Leak_Rate_min = 0, Leak_Rate_max = 0;

    private bool Started = false;

    private OxygenLeak Leak = null;
	// Use this for initialization
	void Start () {
        Leak = GetComponentInChildren<OxygenLeak>();
	}
	
	// Update is called once per frame
	void Update () {

        if(!Started)
        {
            Leak.StartLeak(Start_Raduis_Min, Start_Raduis_Max, Leak_Rate_min, Leak_Rate_max);
            Started = true;
        }
	}

    public override void Activate()
    {
        Success();
    }
    public override void PauseGame(bool pause = true)
    {
        Leak.PauseLeak(pause);
    }

	public override void EventNeeded()
	{
		Collider[] cols = Physics.OverlapSphere(transform.position, 0.25f);
		foreach(Collider col in cols)
		{
			if(col.CompareTag("FixedEvent") && Vector3.Distance(col.transform.position, transform.position) < 0.05f)
			{
				Destroy(col.gameObject);
			}
		}
	}
	
	public override void EventNotNeeded()
	{
		Destroy(this.gameObject);
		//tag = "Untagged";
	}
}

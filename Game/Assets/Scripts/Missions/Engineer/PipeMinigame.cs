using UnityEngine;
using System.Collections;

public class PipeMinigame : Event {

	public GameObject pipe_game;
	private GameObject fixed_prefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Activate ()
	{
		difficulty = GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().ActiveMissionDifficulty();
		GameObject temp = (GameObject)Instantiate(pipe_game);
		temp.transform.position = new Vector3(0, 200, 0);
		temp.GetComponent<PipeGame.BoardManager3D>().Initialise(difficulty, this);
	}

	public override void EventNeeded()
	{
		Collider[] cols = Physics.OverlapSphere(transform.position, 0.25f);
		foreach(Collider col in cols)
		{
			if(col.CompareTag("FixedEvent") && Vector3.Distance(col.transform.position, transform.position) < 0.1f)
			{
				col.gameObject.SetActive(false);
				fixed_prefab = col.gameObject;
				//Destroy(col.gameObject);
			}
		}
	}

	public override void EventNotNeeded()
	{
		Destroy(this.gameObject);
		//tag = "Untagged";
	}

	protected override void OverrideSuccess()
	{
		Destroy(this.gameObject);
		fixed_prefab.SetActive(true);
	}
}

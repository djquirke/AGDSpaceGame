﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDstats : MonoBehaviour {

    [HideInInspector]
	public bool event_close = false;
    [HideInInspector]
    public float HUDOxygen = 1;
	private int event_count, events_done;

    public Text ObjectiveText= null;
    public Text DialogText = null;
    public GameObject DialogBar = null;
    public Text TimeText = null;

    public GameObject OxygenBarMain = null;
    public RectTransform OxygenBarSize = null;
	public GameObject OxygenBarFill = null;
	public GameObject OxygenGraphic = null;
	public GameObject EngineerGraphic = null;
	public GameObject IllnessGraphic = null;
	private GameObject drawSprite = null;
    public Text OxygenText = null;

    private MissionManager mm = null;

	// Use this for initialization
	void Start () {
		
        mm = GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>();

        if(mm.ActiveMissionType() != MissionType.OXYGEN)
        {
            OxygenBarMain.SetActive(false);
        }

		if(mm.ActiveMissionType() == MissionType.ENGINEERING) drawSprite = EngineerGraphic;
		else if(mm.ActiveMissionType() == MissionType.OXYGEN) drawSprite = OxygenGraphic;
		else if(mm.ActiveMissionType() == MissionType.ILLNESS) drawSprite = IllnessGraphic;

		SpawnDrawSprite();
	}

	private void SpawnDrawSprite()
	{
		GameObject placeholder = GameObject.Find("MissionTypeBG");
		RectTransform[] temp = placeholder.GetComponentsInChildren<RectTransform>();
		RectTransform child = temp[1];
//		foreach(RectTransform rt in temp)
//		{
//
//			//Debug.Log("HUD STUFF:" + rt.name);
//		}
		Destroy(child.gameObject);
		GameObject icon = (GameObject)Instantiate(drawSprite);
		icon.transform.SetParent(placeholder.transform);
		icon.transform.localPosition = child.localPosition;
		icon.transform.localScale = child.localScale;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(ObjectiveText)
        {
            ObjectiveText.text = "Events Complete: " + events_done + "/" + event_count;
        }
        if (DialogText && DialogBar)
        {
            if(event_close)
            {
                DialogBar.SetActive(true);
                DialogText.text = "Press E";
            }
            else
            {
                DialogBar.SetActive(false);
            }
        }
        if (TimeText && mm)
        {
            if(mm)
            {
                int time_remaining = Mathf.FloorToInt(mm.TimeRemaining());

                if (time_remaining == -1)
                    return;

                //int time_remaining = MissionManager.MISSION_LENGTH_SECONDS - time_elapsed;
                int mins = Mathf.FloorToInt(time_remaining / 60);
                int secs = time_remaining - mins * 60;

                if (secs < 10)
                   TimeText.text =  mins + ":0" + secs;
                else
                    TimeText.text =  mins + ":" + secs;
            }
        }

        if (OxygenBarSize && OxygenBarFill && OxygenText)
        {
            OxygenBarFill.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, OxygenBarSize.rect.height * HUDOxygen);

            OxygenText.text = Mathf.Floor(HUDOxygen * 100) + "%";

        }

	}

	public void SetNumEvents (int minigame_count)
	{
		event_count = minigame_count;
	}

	public void SetEventsComplete(int count)
	{
		events_done = count;
	}
}

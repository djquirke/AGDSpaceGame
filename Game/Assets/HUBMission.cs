using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUBMission {

	public Mission mission;
	public Vector3 pos;
//	public Button btn;

	public void Initialise(Mission mission, Vector3 pos)
	{
		this.mission = mission;
		this.pos = pos;
//		btn = new Button(new Rect (pos.x, pos.y, 100, 100), mission.missionType ().ToString () + "\n"
//			+ mission.Difficulty ().ToString () + "\n"
//			+ mission.getLevelName ());
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatisticsGrabber : MonoBehaviour {

    public Text Missions_Complete = null;
    public Text Missions_Failed = null;
    public Text Medic_Missions = null;
    public Text Engineer_Mission = null;
    public Text Oxygen_Mission = null;
    public Text Easy_Complete = null;
    public Text Medium_Complete = null;
    public Text Hard_Complete = null;
    public Text Insane_Complete = null;

    public Text HiScore = null;
    
	// Use this for initialization
    void Start()
    {
        if(Missions_Complete)
        {
            Missions_Complete.text = MissionManager.Missions_Won.ToString();
        }
        if (Missions_Failed)
        {
            Missions_Failed.text = MissionManager.Missions_Failed.ToString();
        }
        if (Medic_Missions)
        {
            Medic_Missions.text = MissionManager.Medic_Missions.ToString();
        }
        if (Engineer_Mission)
        {
            Engineer_Mission.text = MissionManager.Engineer_Missions.ToString();
        }
        if (Oxygen_Mission)
        {
            Oxygen_Mission.text = MissionManager.Oxygen_Missions.ToString();
        }
        if (Easy_Complete)
        {
            Easy_Complete.text = MissionManager.Easy_Mission.ToString();
        }
        if (Medium_Complete)
        {
            Medium_Complete.text = MissionManager.Medium_Mission.ToString();
        }
        if (Hard_Complete)
        {
            Hard_Complete.text = MissionManager.Hard_Mission.ToString();
        }
        if (Insane_Complete)
        {
            Insane_Complete.text = MissionManager.Insane_Mission.ToString();
        }
        if (HiScore)
        {
            HiScore.text = MissionManager.HiScore.ToString();
        }
        
    }
}

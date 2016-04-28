using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AfterGameStats : MonoBehaviour {


    public Text MainTitle = null;
    public Text ScoreText = null;
    public Text TimeText = null;
    public Text CompleteText = null;
    public Text FailedText = null;


    private Mission CurrentMission = null;
	// Use this for initialization
	void Start () 
    {
        //get GetCurrentMission
        CurrentMission = FindObjectOfType<MissionManager>().GetCurrentMission();

        if(MainTitle)
        {
            MainTitle.text = CurrentMission.MissionWon() ? "Success" : "Failure";
        }
        if(ScoreText)
        {
			ScoreText.text = CurrentMission.Score().ToString();//((((int)CurrentMission.Difficulty() + 1) * (100 * (int)minigames_complete) - 30 * minigames_failed) * (TimeRemaining() / mission_time)).ToString();

        }
        if(TimeText)
        {
            int time_remaining = Mathf.FloorToInt(CurrentMission.TimeRemaining());

            if (time_remaining == -1)
                return;

            //int time_remaining = MissionManager.MISSION_LENGTH_SECONDS - time_elapsed;
            int mins = Mathf.FloorToInt(time_remaining / 60);
            int secs = time_remaining - mins * 60;

            if (secs < 10)
                TimeText.text = mins + ":0" + secs;
            else
                TimeText.text = mins + ":" + secs;
        }
        if(CompleteText)
        {
            CompleteText.text = CurrentMission.miniGamesDone().ToString();
        }
        if(FailedText)
        {
            FailedText.text = CurrentMission.miniGamesfailed().ToString();
        }

	}
}

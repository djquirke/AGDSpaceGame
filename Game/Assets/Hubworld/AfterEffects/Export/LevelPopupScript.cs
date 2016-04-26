using UnityEngine;
using System.Collections;

public class LevelPopupScript : MonoBehaviour {
    private Animator anim;
	private Difficulty difficulty;
	private MissionType gameMode;
	public Mission mission;
	// Use this for initialization

	public void Initialise (Mission m) {
		mission = m;
		difficulty = m.Difficulty();
		gameMode = m.missionType();
        anim = GetComponent<Animator>();
        StartCoroutine(pushAnimation());
        //pushAnimation(2, 3);
	}

    IEnumerator pushAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        switch (gameMode) {
		case MissionType.OXYGEN:
			switch (difficulty) {
			case Difficulty.Easy:
				anim.Play("Oxygen_1 Easy 2 Spritesheet_Anim");
				break;
			case Difficulty.Medium:
				anim.Play("Oxygen_2 Medium 2 Spritesheet_Anim");
				break;
			case Difficulty.Hard:
				anim.Play("Oxygen_3 Hard 2 Spritesheet_Anim");
				break;
			case Difficulty.Insane:
				anim.Play("Oxygen_4 INSANE 2 Spritesheet_Anim");
				break;
			default:
			break;
			}
			break;
		case MissionType.ENGINEERING:
			switch (difficulty) {
			case Difficulty.Easy:
				anim.Play("Engineering_1 Easy_Spritesheet_Anim");
				break;
			case Difficulty.Medium:
				anim.Play("Engineering_2 Medium 3 Spritesheet_Anim");
				break;
			case Difficulty.Hard:
				anim.Play("Engineering_3 Hard 3 Spritesheet_Anim");
				break;
			case Difficulty.Insane:
				anim.Play("Engineering_4 INSANE Spritesheet_Anim");
				break;
			default:
				break;
			}
			break;
		case MissionType.ILLNESS:
			switch (difficulty) {
			case Difficulty.Easy:
				anim.Play("Sickness_1 Easy Spritesheet_Anim");
				break;
			case Difficulty.Medium:
				anim.Play("Sickness_2 Medium_SPritesheet_Anim");
				break;
			case Difficulty.Hard:
				anim.Play("Sickness_3 Hard Spritesheet_Anim");
				break;
			case Difficulty.Insane:
				anim.Play("Sickness_4 INSANE_Spritesheet_Anim");
				break;
			default:
				break;
			}
			break;
		default:
			break;
		}
    }

	public void MouseClick()
	{
		GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().StartMission(mission.getIdx());
		//Debug.Log("click");
	}
}

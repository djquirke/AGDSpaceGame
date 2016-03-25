using UnityEngine;
using System.Collections;

public class PipeMiniGameAnimationController : MonoBehaviour {
    public GameObject[] GoList;
    void Start()
    {
        openMiniGame();

    }
    public void openMiniGame()
    {
        for (var i = 0; i < GoList.Length; i += 1)
        {
            GoList[i].animation.Play("Door_Open");
        }
    }
    public void closeMiniGame()
    {
        for (var i = 0; i < GoList.Length; i += 1)
        {
            GoList[i].animation.Play("Door_Close");
        }

		StartCoroutine(WaitForAnimation(GoList[0].animation));

		//yield return new WaitForSeconds(GoList[0].animation.clip.length);
    }

	private IEnumerator WaitForAnimation(Animation anim)
	{
		yield return new WaitForSeconds(anim.clip.length + 0.15f);

		transform.root.GetComponent<PipeGame.BoardManager3D>().DoorsClosed();
	}
}

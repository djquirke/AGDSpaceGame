using UnityEngine;
using System.Collections;

public class PipeMiniGameAnimationController : MonoBehaviour {
    public GameObject[] GoList;
    void Start()
    {
        openMiniGame();
    }
    void openMiniGame()
    {
        for (var i = 0; i < GoList.Length; i += 1)
        {
            GoList[i].animation.Play("Door_Open");
        }
    }
    void closeMiniGame()
    {
        for (var i = 0; i < GoList.Length; i += 1)
        {
            GoList[i].animation.Play("Door_Close");
        }
    }
}

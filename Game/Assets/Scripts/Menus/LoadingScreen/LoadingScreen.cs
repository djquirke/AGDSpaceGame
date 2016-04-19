using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LoadingScreen : MonoBehaviour {

    public float dotSpeed = 1;
    public int NumberOfDots = 3;

    private Text LoadingText = null;
    private float timePassed = 0;
    private int count = 0;
	// Use this for initialization
	void Start () {
        LoadingText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        timePassed += Time.deltaTime;
        if(LoadingText)
        {
            LoadingText.text = "Loading";

            for (int idx = 0; idx < count; ++idx )
            {
                LoadingText.text += ".";
            }

            if(timePassed > dotSpeed)
            {
                ++count;
                if(count> NumberOfDots)
                    count = 0;

                timePassed = 0;
            }

        }
	}
}

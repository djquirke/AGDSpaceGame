using UnityEngine;
using System.Collections;

public class FlareController : MonoBehaviour {
    private LensFlare lens;
    public float distanceCap = 0.0f;
    public bool isRandom = false;
    private float OrigBrightness = 0.0f;
    private GameObject player;

	// Use this for initialization
	void Start () {
	}

	public void Initialise()
	{
		lens = GetComponent<LensFlare>();
		if (isRandom)
		{
			lens.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
		}
		OrigBrightness = lens.brightness;
		player = GameObject.FindWithTag("Player");
	}


	// Update is called once per frame
	void Update () {
		if(player)
		{
			float dist = Vector3.Distance(player.transform.position, transform.position);
			if (dist < distanceCap)
			{
				float i = -(dist / distanceCap) + 1;
				lens.brightness = i * OrigBrightness;
			}
		}
	}
}

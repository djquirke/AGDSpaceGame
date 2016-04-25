using UnityEngine;
using System.Collections;

public class CharacterAnimController : MonoBehaviour {
    public float  max, current = 0;
    private float weight = 0.0f;
    private Animator anim;
	void Start () {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {        
        float per = current / max;
        if (per < 0.33) {
            weight = per * 3;

            anim.SetLayerWeight(1, weight);
            anim.SetLayerWeight(2, 0.0f);
        }
        else if (per > 0.33)
        {
            per = Mathf.Min(0.66f, per);
            weight = (per - 0.33f) * 3;
            anim.SetLayerWeight(1, 1.0f);
            anim.SetLayerWeight(2, weight);
        }    
	}
}

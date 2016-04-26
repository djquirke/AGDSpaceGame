using UnityEngine;
using System.Collections;

public class ClothesRackController : MonoBehaviour {
    private Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.Play("Open");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

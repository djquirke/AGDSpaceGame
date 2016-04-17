using UnityEngine;
using System.Collections;

public class OutLineActivation : MonoBehaviour {


    public Shader OutLineShader = null;

    public Color OutLineColour = Color.red; 

    private GameObject Parent = null;
    private Shader ParentsShader = null;
    private bool Triggered = false;


	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(0, 0, 0);
        Parent = transform.parent.gameObject;

        if(!Parent)
        {
            Debug.LogError(name +" has no Parent Object");
        }

        
	}

    // Update is called once per frame
    void Update()
    {
        if(Parent && Parent.tag != "Event")
        {
            if(Triggered)
            {
                Parent.renderer.material.shader = ParentsShader;
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Parent && !Triggered)
        {
            ParentsShader = Parent.renderer.material.shader;
            Parent.renderer.material.shader = OutLineShader;
            Parent.renderer.material.SetColor("_OutlineColor", OutLineColour);
            Triggered = true;
        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && Parent && Triggered)
        {
            Parent.renderer.material.shader = ParentsShader;
            Triggered = false;
        }
    }
}

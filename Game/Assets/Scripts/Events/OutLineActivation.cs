using UnityEngine;
using System.Collections;

public class OutLineActivation : MonoBehaviour {


    public Shader OutLineShader = null;

    public Color OutLineColour = Color.red; 

    public GameObject OutLineObject = null;
    private Shader OutLineObjectsShader = null;
    private bool Triggered = false;


	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(0, 0, 0);
        if(!OutLineObject)
            OutLineObject = transform.parent.gameObject;

        if(!OutLineObject)
        {
            Debug.LogError(name +" has no OutLineObject Object");
        }

        
	}

    // Update is called once per frame
    void Update()
    {
        if(OutLineObject && transform.parent.gameObject.tag != "Event")
        {
            if(Triggered)
            {
                OutLineObject.renderer.material.shader = OutLineObjectsShader;
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && OutLineObject && !Triggered)
        {
            OutLineObjectsShader = OutLineObject.renderer.material.shader;
            OutLineObject.renderer.material.shader = OutLineShader;
            OutLineObject.renderer.material.SetColor("_OutlineColor", OutLineColour);
            Triggered = true;
        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && OutLineObject && Triggered)
        {
            OutLineObject.renderer.material.shader = OutLineObjectsShader;
            Triggered = false;
        }
    }
}

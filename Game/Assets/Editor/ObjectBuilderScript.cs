using UnityEngine;
using UnityEditor;
using System.Collections;

public class ObjectBuilderScript : MonoBehaviour
{
    public GameObject obj;
    public Vector3 spawnPoint;


    public void BuildObject()
    {
		Transform cam_tform = SceneView.lastActiveSceneView.camera.transform;
		GameObject Cam = (GameObject)Instantiate(obj, cam_tform.position, cam_tform.rotation);
        Cam.transform.name = ""+(transform.childCount+1);
		//Cam.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        //Cam.transform.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
        //Cam.transform.Translate(Vector3.forward * -20);
        Cam.transform.parent = transform;
        //Debug.Log(SceneView.lastActiveSceneView.rotation);
        //Debug.Log(Camera.current.transform);
    }
}
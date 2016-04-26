using UnityEngine;
using System.Collections;

public class ShirtSpawner : MonoBehaviour {
    public bool SpawnOrDestroy = false;
    private bool isBox = false;
    public GameObject targetZone;
    public GameObject box;
    // Use this for initialization
    void Start()
    {

        StartCoroutine(Delayed());

        /*
       Rigidbody rigid = box2.gameObject.AddComponent("Rigidbody") as Rigidbody;
       BoxCollider boxCol = box2.gameObject.AddComponent("BoxCollider") as BoxCollider;
       rigid.useGravity = false;
       rigid.constraints = RigidbodyConstraints.FreezeRotation;
       rigid.constraints = RigidbodyConstraints.FreezePosition;
       sc.isBox = true;
        * */


    }
    IEnumerator Delayed()
    {

        if (!isBox & SpawnOrDestroy)
        {
            GameObject box2 = Instantiate(box, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            box2.transform.parent = transform;


            box2.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, (transform.position.z));
            box2.transform.localPosition -= new Vector3(0.28f, 0f, 0f);
            box2.transform.rotation = transform.rotation;
            ShirtSpawner sc = box2.gameObject.AddComponent("ShirtSpawner") as ShirtSpawner;
            sc.isBox = true;
            sc.targetZone = targetZone;

            //
            yield return new WaitForSeconds(Random.Range(1, 3));
            StartCoroutine(Delayed());
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (isBox)
        {
            float dist = Vector3.Distance(targetZone.transform.position, transform.position);
            if (dist < 0.6)
            {
                Destroy(gameObject);
            }
            transform.localPosition -= new Vector3(0.005f, 0f, 0f);
        }
    }
}

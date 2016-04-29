using UnityEngine;
using System.Collections;
 using System.Collections.Generic;
using System.Collections.Generic;
public class DecalController : MonoBehaviour {
    public bool SpriteOverride = false;
    public Sprite SpriteImage;
    public Sprite[] RoomNumbers;
    public float gen = 0.0f;
    public List<float> genList = new List<float>();
    public bool locked = false;
	// Use this for initialization
	void Start () {
		if (SpriteImage)
        {
            SpriteRenderer spt = GetComponent<SpriteRenderer>();
            spt.sprite = SpriteImage;
        }
        else
        {

            gen = Random.Range(0.0f, 0.1f);
            StartCoroutine(sortDecals());
            //GameObject[] DecalNumbers = GameObject.FindGameObjectsWithTag("Decal");
            //foreach (GameObject DecalObj in DecalNumbers) {
            //    DecalController targetScript = DecalObj.GetComponent<DecalController>();
            //    if
            //}
        }
	}
    IEnumerator sortDecals()
    {
        
        yield return new WaitForSeconds(2);
        GameObject[] DecalNumbers = GameObject.FindGameObjectsWithTag("Decal");
		Debug.Log ("DECALS FOUND: " + DecalNumbers.Length);
		if(DecalNumbers.Length == 0) yield break;
        foreach (GameObject DecalObj in DecalNumbers)
        {
            DecalController targetScript = DecalObj.GetComponent<DecalController>() as DecalController;
            if (targetScript && !targetScript.SpriteOverride)
            {
                genList.Add(targetScript.gen);
            }
        }
        genList.Sort();
        int i = 1;
        while (i < genList.Count && i < RoomNumbers.Length)
        {
            Debug.Log(genList[i]);
            if (genList[i] == gen)
            {
                SpriteRenderer spt = GetComponent<SpriteRenderer>();
                spt.sprite = RoomNumbers[i];
                locked = true;
            }
            i += 1;
        }
        if (!locked)
        {
            Destroy(gameObject);
        }
        /*
        yield return new WaitForSeconds(1);
        GameObject[] DecalNumbers = GameObject.FindGameObjectsWithTag("Decal");
        genlist = new float[DecalNumbers.Length];
        int i = 0;
        foreach (GameObject DecalObj in DecalNumbers)
        {

            DecalController targetScript = DecalObj.GetComponent<DecalController>() as DecalController;
            if (!targetScript.SpriteOverride)
            {
                genlist[i] = targetScript.gen;
            }
            i+=1;
        }
        i = 0;
        System.Array.Sort(genlist);
        while (i < RoomNumbers.Length && i < genlist.Length)
        {
            if (genlist[i] == gen)
            {
                SpriteRenderer spt = GetComponent<SpriteRenderer>();
                spt.sprite = RoomNumbers[i];
                locked = true;
            }
            i += 1;
            
        }
        if (!locked)
        {
            Destroy(gameObject);
        }
         * */
        Debug.Log(transform.name + "  Decal Done");
    }
	// Update is called once per frame
	void Update () {
	
	}
}

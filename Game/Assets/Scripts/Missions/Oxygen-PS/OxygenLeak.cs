using UnityEngine;
using System.Collections;

public class OxygenLeak : MonoBehaviour {

   


    private float Leak_Rate = 0;
    private SphereCollider Area_of_Effect;
    private bool isRunning = false;
	// Use this for initialization
	void Start () {
        Area_of_Effect = GetComponent<SphereCollider>();    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(isRunning)
        {

            if (transform.parent.gameObject.tag == "Event")
            {
                Area_of_Effect.radius += Leak_Rate * Time.deltaTime;
            }
            else
            {
                Area_of_Effect.radius -= Time.deltaTime;

                if(Area_of_Effect.radius <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
	}

    public void StartLeak( float Start_Raduis_Min, float Start_Raduis_Max, float Leak_Rate_min, float Leak_Rate_max)
    {
         Area_of_Effect.radius = Random.Range(Start_Raduis_Min, Start_Raduis_Max);

        Leak_Rate = Random.Range(Leak_Rate_min, Leak_Rate_max);  
     
        isRunning = true;

    }

    public void PauseLeak(bool pause = true)
    {
        isRunning = !pause;
    }
}

using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {

    public EventBase EventCode = null;


	void OnTriggerEnter(Collider Other)
    {
        EventCode.Activate();
    }
}

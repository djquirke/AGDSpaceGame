using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class QuickTimeEvent : MonoBehaviour {
    public enum ControllerInput
    {
        Y,
        X,
        A,
        B,
        RB,
        LB,
        RT,
        LT,
        D_HatDown,
        D_HatUp,
        D_HatLeft,
        D_HatRight,
        LeftStick,
        Rightstick,
		Num_of_Buttons
    };

    public List<KeyCode>  m_Key_List;
	public List<ControllerInput> m_Button_List;
    


    public bool m_IsRandemOrder = false;
	public bool m_IsRandomButtons = false;
    public int m_Number_of_events = 0;



    
	// Use this for initialization
	void Start () 
    {
		//Check that the numebr of event isn't negative
		m_Number_of_events = (m_Number_of_events > 0) ? m_Number_of_events
												  	  : 0;
		if (m_IsRandomButtons) {
			m_Key_List.Clear ();
			m_Button_List.Clear ();
			System.Random Rand = new System.Random ();
			for (int x= 0; x < m_Number_of_events; ++x) {
				m_Button_List.Add ((ControllerInput)Rand.Next ((int)ControllerInput.Num_of_Buttons));

				m_Key_List.Add ((KeyCode)Rand.Next ((int)KeyCode.A, (int)KeyCode.Z));
			}
		} 
		else if (m_IsRandemOrder)
		{

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

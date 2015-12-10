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

    public List<KeyCode>  m_KeyList;
	public List<ControllerInput> m_ButtonList;
    
	public int TimeBetweenEvents = 2000;

    public bool m_IsRandomOrder = false;
	public bool m_IsRandomButtons = false;
    public int m_Number_of_events = 0;


	private bool m_isActive = false;

	private List<KeyCode>  m_ActiveKeyList;
	private List<ControllerInput> m_ActiveButtonList;

    private int TimePassed;

    
	// Use this for initialization
	void Start () 
    {
		//Check that the numebr of event isn't negative
		m_Number_of_events = (m_Number_of_events > 0) ? m_Number_of_events
												  	  : 0;

		//if random buttons is 
		if (m_IsRandomButtons) 
		{
			m_Key_List.Clear ();
			m_Button_List.Clear ();
			System.Random Rand = new System.Random ();
			for (int x= 0; x < m_Number_of_events; ++x) {
				m_Button_List.Add ((ControllerInput)Rand.Next ((int)ControllerInput.Num_of_Buttons));

				m_Key_List.Add ((KeyCode)Rand.Next ((int)KeyCode.A, (int)KeyCode.Z));
			}
		} 
		else if (m_IsRandomOrder)
		{
			System.Random Rand = new System.Random ();
			List<ControllerInput> NewButtonList;
			List<KeyCode> NewKeyList;

			//randimize the Controllor buttons
			while(m_Button_List.Count)
			{
				int randIndex = Rand.Next(m_Button_list.Count);

				TempNewList.Add(m_Button_List[randIndex]);
				m_Button_List.RemoveAt(randIndex);
			}
			//randimize the KeyValuePair board buttons
			while(m_Key_List.Count)
			{
				int randIndex = Rand.Next(m_Key_List.Count);
				
				TempNewList.Add(m_Key_List[randIndex]);
				m_Key_List.RemoveAt(randIndex);
			}

			m_Button_List = NewButtonList;
			m_Key_List = NewKeyList;

		}
		m_ActiveKeyList = m_KeyList;
		m_ActiveButtonList = m_ButtonList;

	}
	
	// Update is called once per frame
	void Update () 
	{

		if (m_isActive) 
		{
			
		}
	
	}

	public void Activate()
	{
		m_isActive = true;
	}

	public bool GetisActive()
	{
		return  m_isActive;
	}
}

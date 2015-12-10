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

    public List<KeyCode>  m_KeyList = new List<KeyCode>();
	public List<ControllerInput> m_ButtonList = new List<ControllerInput>();
    
	public float TimeBetweenEvents = 2f;

    public bool m_IsRandomOrder = false;
	public bool m_IsRandomButtons = false;
    public int m_Number_of_events = 0;


	private bool m_isActive = false;

    private List<KeyCode> m_ActiveKeyList = new List<KeyCode>();
    private List<ControllerInput> m_ActiveButtonList = new List<ControllerInput>();

    private float m_fTimePassed = 0f;


    //controller setup
    private GamePadState m_gpState;
    private GamePadState m_gpPrevState;
    private PlayerIndex m_PlayerIndex;
    private bool m_bPlayerIndexSet = false;
    
    
	// Use this for initialization
	void Start () 
    {
		//Check that the numebr of event isn't negative
		m_Number_of_events = (m_Number_of_events > 0) ? m_Number_of_events
												  	  : 0;

		//if random buttons is 
		if (m_IsRandomButtons) 
		{
            m_KeyList.Clear();
            m_ButtonList.Clear();
			System.Random Rand = new System.Random ();
			for (int x= 0; x < m_Number_of_events; ++x) {
                m_ButtonList.Add((ControllerInput)Rand.Next((int)ControllerInput.Num_of_Buttons));

                m_KeyList.Add((KeyCode)Rand.Next((int)KeyCode.A, (int)KeyCode.Z));
			}
		} 
		else if (m_IsRandomOrder)
		{
			System.Random Rand = new System.Random ();
			List<ControllerInput> NewButtonList = new List<ControllerInput>();
            List<KeyCode> NewKeyList = new List<KeyCode>();

			//randimize the Controllor buttons
            while (m_ButtonList.Count >0)
			{
                int randIndex = Rand.Next(m_ButtonList.Count);

                NewButtonList.Add(m_ButtonList[randIndex]);
                m_ButtonList.RemoveAt(randIndex);
			}
			//randimize the KeyValuePair board buttons
            while (m_KeyList.Count>0)
			{
                int randIndex = Rand.Next(m_KeyList.Count);

                NewKeyList.Add(m_KeyList[randIndex]);
                m_KeyList.RemoveAt(randIndex);
			}
            m_ButtonList.Clear();
            m_ButtonList.AddRange(NewButtonList);
            m_KeyList.Clear();
            m_KeyList .AddRange(NewKeyList);

		}
		m_ActiveKeyList = m_KeyList;
		m_ActiveButtonList = m_ButtonList;

	}
	
	// Update is called once per frame
	void Update () 
	{
         if (!m_bPlayerIndexSet || !m_gpPrevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    m_PlayerIndex = testPlayerIndex;
                    m_bPlayerIndexSet = true;
                }
            }
        }

        m_gpPrevState = m_gpState;
        m_gpState = GamePad.GetState(m_PlayerIndex);

		if (m_isActive ) 
		{
           // if(m_gpState.isConnected && m_ButtonList.Count >0)
            {
                if (m_fTimePassed < TimeBetweenEvents)
                  {
                     // m_gpState.
                  }
                  else
                  {

                  }
            }
            //else if(m_KeyList.Count >0)
            {
                if (m_fTimePassed < TimeBetweenEvents)
                {
			        //if(Input.GetKeyDown(m_));
                }
            }
            //else
            {
                m_isActive = false;
                m_fTimePassed = 0f;
            }

            m_fTimePassed += Time.deltaTime;


		}
	
	}

	public void Activate()
	{
		m_isActive = true;
        m_fTimePassed = 0f;
	}

	public bool GetisActive()
	{
		return  m_isActive;
	}
}

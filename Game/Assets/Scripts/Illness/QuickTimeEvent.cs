using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class QuickTimeEvent : EventBase {
    public enum ControllerInput
    {
        Y,
        X,
        A,
        B,
        RB,
        LB,
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

    public Vector2 m_UI_Pos = new Vector2(0.5f,0.5f);
    public Vector2 m_UI_Size = new Vector2(256f, 50f);


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
            if (m_gpState.IsConnected && m_ActiveButtonList.Count > 0)
            {
                if (m_fTimePassed < TimeBetweenEvents)
                  {
                      if (CheckControllerButton(m_ActiveButtonList[0]))
                    {
                        m_ActiveButtonList.RemoveAt(0);
                    }
                  }
                  else
                  {
                    // missed do something
                  }
            }
            else if (m_ActiveKeyList.Count > 0)
            {
                if (m_fTimePassed < TimeBetweenEvents)
                {
                    if (Input.GetKeyDown(m_ActiveKeyList[0]))
                    {
                        m_ActiveKeyList.RemoveAt(0);
                    }
                }
                else
                {
                    // missed do somthing
                }
            }
            else
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

    private bool CheckControllerButton(ControllerInput CorrectInput)
    {
        if(!m_gpState.IsConnected)
        {
            return false;
        }

        bool bCorrect = false;
        
        switch(CorrectInput)
        {
            case(ControllerInput.Y):
                if(m_gpPrevState.Buttons.Y == ButtonState.Released && m_gpState.Buttons.Y == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.X):
                if (m_gpPrevState.Buttons.X == ButtonState.Released && m_gpState.Buttons.X == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.A):
                if (m_gpPrevState.Buttons.A == ButtonState.Released && m_gpState.Buttons.A == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.B):
                if (m_gpPrevState.Buttons.B == ButtonState.Released && m_gpState.Buttons.B == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.RB):
                if (m_gpPrevState.Buttons.RightShoulder == ButtonState.Released && m_gpState.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.LB):
                if (m_gpPrevState.Buttons.LeftShoulder == ButtonState.Released && m_gpState.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.D_HatDown):
                if (m_gpPrevState.DPad.Down == ButtonState.Released && m_gpState.DPad.Down == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.D_HatLeft):
                if (m_gpPrevState.DPad.Left == ButtonState.Released && m_gpState.DPad.Left == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.D_HatRight):
                if (m_gpPrevState.DPad.Right == ButtonState.Released && m_gpState.DPad.Right == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.D_HatUp):
                if (m_gpPrevState.DPad.Up == ButtonState.Released && m_gpState.DPad.Up == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.LeftStick):
                if (m_gpPrevState.Buttons.LeftStick == ButtonState.Released && m_gpState.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;
            case (ControllerInput.Rightstick):
                if (m_gpPrevState.Buttons.RightStick == ButtonState.Released && m_gpState.Buttons.RightStick == ButtonState.Pressed)
                {
                    bCorrect = true;
                }
                break;

        }

        return bCorrect;

    }

    void OnGUI()
    {
        if (m_isActive)
        {
            if (m_gpState.IsConnected && m_ActiveButtonList.Count > 0)
            {
               GUI.Box(new Rect(Camera.main.GetScreenWidth() * m_UI_Pos.x,
                                Camera.main.GetScreenHeight() * m_UI_Pos.y,
                                m_UI_Size.x, m_UI_Size.y), "");
            }
            else if (m_KeyList.Count > 0)
            {
               GUI.Box(new Rect(Camera.main.GetScreenWidth() * m_UI_Pos.x,
                                Camera.main.GetScreenHeight() * m_UI_Pos.y,
                                m_UI_Size.x, m_UI_Size.y), "Press " + m_ActiveKeyList[0]);
            }
            
        }
    }

   virtual public void Activate(bool activate = true)
    {
       // if it is not active reset the lists
        if (m_isActive)
        {
            m_ActiveButtonList = m_ButtonList;
            m_ActiveKeyList = m_KeyList;
        }

        m_isActive = activate;
    }
    //private string GetButtonString(ControllerInput)
    //{

    //}

}

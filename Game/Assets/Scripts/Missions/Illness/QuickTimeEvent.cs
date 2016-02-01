using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class QuickTimeEvent : Event {
    public enum ControllerInput
    {
        Y,
        X,
        A,
        B,
        D_HatDown,
        D_HatUp,
        D_HatLeft,
        D_HatRight,
        Num_of_Buttons
    };
    public enum KeyBoardInput
    {
        W,
        A,
        S,
        D,
        UpArrow,
        RightArrow,
        LeftArrow,
        DownArrow,
        Num_of_Keys
    }

    //Keyboard WASD, Arrow,

    public List<KeyBoardInput> m_KeyList = new List<KeyBoardInput>();
	public List<ControllerInput> m_ButtonList = new List<ControllerInput>();
    
	public float TimeBetweenEvents = 2f;

    public bool m_IsRandomOrder = false;
	public bool m_IsRandomButtons = false;
    public int m_Number_of_events = 0;

    public Vector2 m_UI_Pos = new Vector2(0.5f,0.5f);
    public Vector2 m_UI_Size = new Vector2(128f, 128f);

    public List<Texture> m_xBoxTex = new List<Texture>();
    public List<Texture> m_KeyTex = new List<Texture>();

	private bool m_isActive = false;

    private List<KeyBoardInput> m_ActiveKeyList = new List<KeyBoardInput>();
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

                m_KeyList.Add((KeyBoardInput)Rand.Next((int)KeyBoardInput.Num_of_Keys));
			}
		} 
		else if (m_IsRandomOrder)
		{
			System.Random Rand = new System.Random ();
			List<ControllerInput> NewButtonList = new List<ControllerInput>();
            List<KeyBoardInput> NewKeyList = new List<KeyBoardInput>();

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
                        m_fTimePassed = 0f;
                    }
                  }
                  else
                  {
                    // missed do something
                  }
            }
            else if (!m_gpState.IsConnected && m_ActiveKeyList.Count > 0)
            {
                if (m_fTimePassed < TimeBetweenEvents)
                {
                    if (CheckKeys(m_ActiveKeyList[0]))
                    {
                        m_ActiveKeyList.RemoveAt(0);
                        m_fTimePassed = 0f;
                    }
                }
                else
                {
                    // missed do somthing
                }
            }
            else
            {
                if(m_ButtonList.Count > 0 && m_ActiveButtonList.Count == 0)
                {
                    //win
                    Success();

                }
                else if(m_KeyList.Count > 0 && m_ActiveKeyList.Count == 0)
                {
                    //win
                    Success();
                }
                else
                {
                    //fail
                    Failure();
                }
                m_isActive = false;
                m_fTimePassed = 0f;
                FindObjectOfType<Player_Movement>().EnablePlayerMovement();
            }

            m_fTimePassed += Time.deltaTime;


		}
	
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
        }

        return bCorrect;

    }

    private bool CheckKeys(KeyBoardInput CorrectInput)
    {
        bool bCorrect = false;

        switch (CorrectInput)
        {
            case (KeyBoardInput.W):
                if (Input.GetKeyDown(KeyCode.W))
                {
                    bCorrect = true;
                }
                break;
            case (KeyBoardInput.A):
                if (Input.GetKeyDown(KeyCode.A))
                {
                    bCorrect = true;
                }
                break;
            case (KeyBoardInput.S):
                if (Input.GetKeyDown(KeyCode.S))
                {
                    bCorrect = true;
                }
                break;
            case (KeyBoardInput.D):
                if (Input.GetKeyDown(KeyCode.D))
                {
                    bCorrect = true;
                }
                break;
            case (KeyBoardInput.UpArrow):
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    bCorrect = true;
                }
                break;
            case (KeyBoardInput.RightArrow):
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    bCorrect = true;
                }
                break;
            case (KeyBoardInput.DownArrow):
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    bCorrect = true;
                }
                break;
            case (KeyBoardInput.LeftArrow):
                if (Input.GetKeyDown(KeyCode.LeftArrow))
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
               GUI.DrawTexture(new Rect(Screen.width * m_UI_Pos.x - (m_UI_Size.x/2),
                                        Screen.height * m_UI_Pos.y - (m_UI_Size.y /2),
                                       m_UI_Size.x,
                                        m_UI_Size.y),
                               m_xBoxTex[(int)m_ActiveButtonList[0]]);
            }
            else if (m_KeyList.Count > 0 && m_ActiveButtonList.Count > 0)
            {
                GUI.DrawTexture(new Rect(Screen.width * m_UI_Pos.x - (m_UI_Size.x / 2),
                                         Screen.height * m_UI_Pos.y - (m_UI_Size.y / 2),
                                         m_UI_Size.x,
                                         m_UI_Size.y),
                                m_KeyTex[(int)m_ActiveKeyList[0]]);
            }
            
        }
    }

    public override void Activate()
    {
       // if it is not active reset the lists
       
            m_ActiveButtonList = m_ButtonList;
            m_ActiveKeyList = m_KeyList;
        

        m_isActive = true;

        m_fTimePassed = 0f;
	}
    
 
}

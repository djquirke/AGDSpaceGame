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

    private List<KeyBoardInput> m_KeyList = new List<KeyBoardInput>();
    private List<ControllerInput> m_ButtonList = new List<ControllerInput>();

    public float EasyReactionTime = 4f;
    public float MedReactionTime = 2f;
    public float HardReactionTime = 1f;

    public int EasyNumberofEvents = 2;
    public int MedNumberofEvents = 4;
    public int HardNumberofEvents = 6;


    private float m_reaction_time = 2f;

    private int m_Number_of_events = 0;

    public Vector2 m_UI_Pos = new Vector2(0.5f,0.5f);
    public Vector2 m_UI_Size = new Vector2(128f, 128f);

    public List<Texture> m_xBoxTex = new List<Texture>();
    public List<Texture> m_KeyTex = new List<Texture>();

    public Material IncompleteMat;
    public Material CompleteMat;
	public GameObject finished_effect;

	private bool m_isActive = false;

    private List<KeyBoardInput> m_ActiveKeyList = new List<KeyBoardInput>();
    private List<ControllerInput> m_ActiveButtonList = new List<ControllerInput>();
    
    private float m_fTimePassed = 0f;

    private bool isPaused = false;


    //controller setup
    private GamePadState m_gpState;
    private GamePadState m_gpPrevState;
    private PlayerIndex m_PlayerIndex;
    private bool m_bPlayerIndexSet = false;

	public override void EventNotNeeded()
	{
		tag = "Untagged";
		//transform.tag = "Untagged";
		GetComponentInChildren<Renderer>().material = CompleteMat;
        SetMaterial(CompleteMat);
	}
    
	// Use this for initialization
	void setup() 
    {
        Difficulty d = GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().ActiveMissionDifficulty();
        switch (d)
        {
            case Difficulty.Easy:
                {
                    m_reaction_time = EasyReactionTime;
                    m_Number_of_events = EasyNumberofEvents;
                    break;
                }
            case Difficulty.Medium:
                {
                    m_reaction_time = MedReactionTime;
                    m_Number_of_events = MedNumberofEvents;
                    break;
                }
            default:
                {
                    m_reaction_time = HardReactionTime;
                    m_Number_of_events = HardNumberofEvents;
                    break;
                }

        }
       
		//Check that the numebr of event isn't negative
		m_Number_of_events = (m_Number_of_events > 0) ? m_Number_of_events
												  	  : 0;

		//set random buttons
		
        m_KeyList.Clear();
        m_ButtonList.Clear();
		//System.Random Rand = new System.Random (Random.);v
		for (int x= 0; x < m_Number_of_events; ++x) 
        {
            m_ButtonList.Add((ControllerInput)Random.Range(0,(int)ControllerInput.Num_of_Buttons));//(ControllerInput)Rand.Next((int)ControllerInput.Num_of_Buttons));

            m_KeyList.Add((KeyBoardInput)Random.Range(0,(int)KeyBoardInput.Num_of_Keys));//(KeyBoardInput)Rand.Next((int)KeyBoardInput.Num_of_Keys));
		}
		 

		//randomise button order
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

		

		m_ActiveKeyList.AddRange(m_KeyList);
		m_ActiveButtonList.AddRange(m_ButtonList);


        SetMaterial(IncompleteMat);
        //GetComponentInChildren<Renderer>().material = IncompleteMat;

	}
	
	// Update is called once per frame
	void Update () 
	{
        if (isPaused)
            return;

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
            if (m_gpState.IsConnected && m_ActiveButtonList.Count > 0 && m_fTimePassed < m_reaction_time)
            {
                
                if (CheckControllerButton(m_ActiveButtonList[0]))
                {
                    m_ActiveButtonList.RemoveAt(0);
                    m_fTimePassed = 0f;
                }
                  
                 
            }
            else if (!m_gpState.IsConnected && m_ActiveKeyList.Count > 0 && m_fTimePassed < m_reaction_time)
            {
                if (CheckKeys(m_ActiveKeyList[0]))
                {
                    m_ActiveKeyList.RemoveAt(0);
                    m_fTimePassed = 0f;
                }
                
            }
            else
            {
                if(m_ButtonList.Count > 0 && m_ActiveButtonList.Count == 0)
                {
                    //win
					m_isActive = false;
                    Success();
					StartCoroutine(RestoreCharacter());
                   // GetComponentInChildren<Renderer>().material = CompleteMat;

                }
                else if(m_KeyList.Count > 0 && m_ActiveKeyList.Count == 0)
                {
                    //win
					m_isActive = false;
					Success();
					StartCoroutine(RestoreCharacter());
                    //GetComponentInChildren<Renderer>().material = CompleteMat;
                }
                else
                {
                    //fail
					m_isActive = false;
                    Failure();
                }

                
                m_fTimePassed = 0f;
//                FindObjectOfType<Player_Movement>().EnablePlayerMovement();
            }

            m_fTimePassed += Time.deltaTime;


		}
	
	}

	
	private IEnumerator RestoreCharacter()
	{
		SetMaterial(CompleteMat);
		//transform.parent.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
		Destroy(transform.parent.gameObject.GetComponentInChildren<ParticleSystem>().gameObject);
		yield return new WaitForEndOfFrame();

		GameObject obj = (GameObject)Instantiate(finished_effect, this.transform.position, this.transform.rotation);
		obj.transform.Rotate(new Vector3(270, 0, 0));
		obj.transform.SetParent(transform.root);
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
        if (m_isActive && !isPaused)
        {
            if (m_gpState.IsConnected && m_ActiveButtonList.Count > 0)
            {
               GUI.DrawTexture(new Rect(Screen.width * m_UI_Pos.x - (m_UI_Size.x/2),
                                        Screen.height * m_UI_Pos.y - (m_UI_Size.y /2),
                                       m_UI_Size.x,
                                        m_UI_Size.y),
                               m_xBoxTex[(int)m_ActiveButtonList[0]]);
            }
            else if (m_KeyList.Count > 0 && m_ActiveKeyList.Count > 0)
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
       
        setup();


        m_ActiveKeyList.Clear();
        m_ActiveButtonList.Clear();

        m_ActiveKeyList.AddRange(m_KeyList);
        m_ActiveButtonList.AddRange(m_ButtonList);
        

        m_isActive = true;

        m_fTimePassed = 0f;
	}
    public override void PauseGame(bool pause = true)
    {
        isPaused = pause;
    }
 
    private void SetMaterial(Material mat)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            r.material = mat; 
        }
    }
}

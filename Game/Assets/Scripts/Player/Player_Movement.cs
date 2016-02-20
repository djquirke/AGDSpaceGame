using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Player_Movement : MonoBehaviour {

    

   // public float TurnSpeed_Mulitplier = 1.0f;
    public float MoveSpeed_mulitplier = 1.0f;


    private GameObject Front;

    private Vector3 m_rotationAxis;

    private GamePadState m_gpState;
    private GamePadState m_gpPrevState;
    private PlayerIndex m_PlayerIndex;
    private bool m_bPlayerIndexSet = false;

    private bool m_bPlayerCanMove = true;

    private Vector3 m_TargetRotation, m_StartRotation;
    private Vector3 Rotatevelocity = Vector3.zero;
    public float Roation_time = 0.5f;


	// Use this for initialization
	void Start () {

        Front = GameObject.Find("Front");
        m_rotationAxis.y = 1;
        m_rotationAxis.x = 0;
        m_rotationAxis.z = 0;

        m_TargetRotation = transform.rotation.eulerAngles;
        m_StartRotation = m_TargetRotation;
	
	}
	
	// Update is called once per frame
	void Update () {

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

        if (m_bPlayerCanMove)
        {

            float LeftStick_xAxis = m_gpState.ThumbSticks.Left.X;
            float LeftStick_yAxis = m_gpState.ThumbSticks.Left.Y;

            if (Input.GetKey(KeyCode.W))
            {
                LeftStick_yAxis = 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                LeftStick_yAxis = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                LeftStick_xAxis = 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                LeftStick_xAxis = -1;
            }

            if ((m_gpPrevState.Buttons.RightShoulder == ButtonState.Released && m_gpState.Buttons.RightShoulder == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.Q))
            {
                m_StartRotation = transform.rotation.eulerAngles;
                m_TargetRotation = m_StartRotation + (m_rotationAxis * 90);

            }
            else if ((m_gpPrevState.Buttons.LeftShoulder == ButtonState.Released && m_gpState.Buttons.LeftShoulder == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.E))
            {
                m_StartRotation = transform.rotation.eulerAngles;
                m_TargetRotation = m_StartRotation + (m_rotationAxis * -90);

            }

            Vector3 rotation = Vector3.SmoothDamp(m_StartRotation, m_TargetRotation, ref Rotatevelocity, Roation_time);

            m_StartRotation = rotation;

            transform.rotation = Quaternion.Euler(rotation);


            Vector3 Direction = Front.transform.position - transform.position;

            Direction.Normalize();

            Vector3 DirectionRight = new Vector3(Direction.z, Direction.y, -Direction.x);

            DirectionRight *= LeftStick_xAxis;
            Direction *= LeftStick_yAxis;
            Vector3 TotalDirection = Direction + DirectionRight;

            TotalDirection.Normalize();

            TotalDirection *= Time.deltaTime;

            transform.position += (TotalDirection) * MoveSpeed_mulitplier;
        }
	}


    void OnTriggerStay(Collider other)
    {

        if ((m_gpPrevState.Buttons.A == ButtonState.Released && m_gpState.Buttons.A == ButtonState.Pressed) || Input.GetMouseButtonDown(0))
        {
            if (other.tag.Equals("Event") && other.GetComponent<Event>())
            {
              
                other.GetComponent<Event>().Activate();
                EnablePlayerMovement(false);
               
            }
        }


    }

    public void EnablePlayerMovement(bool bCanmove = true)
    {
        m_bPlayerCanMove = bCanmove;
        GetComponentInChildren<Camera_Movement>().Enable(bCanmove);
    }
}

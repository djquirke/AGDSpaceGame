using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Player_Movement : MonoBehaviour {

    

   // public float TurnSpeed_Mulitplier = 1.0f;
    public float MoveSpeed_mulitplier = 1.0f;


    private GameObject Front;

    private Vector3 m_rotationAxis;

    private GamePadState state;
    private GamePadState prevState;
    private PlayerIndex playerIndex;
    private bool playerIndexSet = false;


	// Use this for initialization
	void Start () {

        Front = GameObject.Find("Front");
        m_rotationAxis.y = 1;
        m_rotationAxis.x = 0;
        m_rotationAxis.z = 0;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);

        float LeftStick_xAxis = state.ThumbSticks.Left.X;
        float LeftStick_yAxis = state.ThumbSticks.Left.Y;  
    
        if(Input.GetKey(KeyCode.W))
        {
            LeftStick_yAxis = 1;
        }
        if(Input.GetKey(KeyCode.S))
        {
            LeftStick_yAxis = -1;
        }
        if(Input.GetKey(KeyCode.D))
        {
            LeftStick_xAxis = 1;
        }
        if(Input.GetKey(KeyCode.A))
        {
            LeftStick_xAxis = -1;
        }

        if ((prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(m_rotationAxis, 90);
        }
        else if ((prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(m_rotationAxis, -90);
        }
        //transform.Rotate(m_rotationAxis, RightStick_xAxis * TurnSpeed_Mulitplier);

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

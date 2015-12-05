using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Camera_Movement : MonoBehaviour {


    public float LookSpeed_mulitplier = 1.0f;
    public float max_Vertical_Angle = 90.0f;
    public float min_Vertical_Angle = .0f;
    public float Close_Zoom_Dist = 5.0f;
    public float Far_Zoom_Dist = 20.0f;

    public GameObject Camera_object = null;


    private Vector3 m_rotationYAxis;
    private Vector3 m_rotationXAxis;
    private Vector3 m_rotationZAxis;
    private float m_Angle;
    private bool m_zoomedIn = false;

    private GamePadState state;
    private GamePadState prevState;
    private PlayerIndex playerIndex;
    private bool playerIndexSet = false;

	// Use this for initialization
	void Start () {

        //Y- axis set up
        m_rotationYAxis.x = 0;
        m_rotationYAxis.y = 1;
        m_rotationYAxis.z = 0;

        //x- axis set up
        m_rotationXAxis.x = 0;
        m_rotationXAxis.y = 0;
        m_rotationXAxis.z = 1;

        //z-axis set up
        m_rotationZAxis.x = 0;
        m_rotationZAxis.y = 0;
        m_rotationZAxis.z = 1;
        m_Angle = transform.eulerAngles.z;
	}

    // Update is called once per frame
    void Update()
    {

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




        float RightStick_yAxis = state.ThumbSticks.Right.Y;


        //transform.Rotate(m_rotationXAxis, RightStick_yAxis * LookSpeed_mulitplier);
        m_Angle += RightStick_yAxis * LookSpeed_mulitplier;

        if (m_Angle < min_Vertical_Angle)
        {
            m_Angle = min_Vertical_Angle;
        }
        else if (m_Angle > max_Vertical_Angle)
        {
            m_Angle = max_Vertical_Angle;
        }

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, m_Angle);

        if (Camera_object != null)
        {

            if (Input.GetKeyDown(KeyCode.Joystick1Button3))
            {

                if (m_zoomedIn)
                {
                    Camera_object.transform.localPosition = new Vector3(Far_Zoom_Dist, 0, 0);
                }
                else
                {
                    Camera_object.transform.localPosition = new Vector3(Close_Zoom_Dist, 0, 0);
                }

                m_zoomedIn = !m_zoomedIn;
            }
        }
       
    }
}

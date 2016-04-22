using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class Player_Movement : MonoBehaviour {

    

   // public float TurnSpeed_Mulitplier = 1.0f;
    public float MoveSpeed_mulitplier = 1.0f;
    public float RunSpeed_mulitplier = 2.0f;


    private GameObject Front, mesh;

    private Vector3 m_rotationAxis;

    private GamePadState m_gpState;
    private GamePadState m_gpPrevState;
    private PlayerIndex m_PlayerIndex;
    private bool m_bPlayerIndexSet = false;

    private bool m_bPlayerCanMove = true;
    private bool rotate = false;

    private Vector3 m_TargetRotation, m_StartRotation;
    private Vector3 Rotatevelocity = Vector3.zero, CharRotateVelocity = Vector3.zero;

    private Vector3 m_CharictorTotalRotation = Vector3.zero;
    private Vector3 m_CharictorTargetRotation = Vector3.zero;
    private Vector3 m_MeshOffsetRotation = Vector3.zero;
    public float Roation_time = 0.5f, Char_Rotation_Time = 0.1f;

    public int OxygenSamples = 50;
    private float CurOxygenLevel = 1;
    private List<float> OxyageAv = new List<float>();

    private bool m_bWasActive; 

    private bool isRunning = false;


	// Use this for initialization
	void Start () {

        Front = GameObject.Find("Front");
        mesh = GameObject.Find("PlayerMesh");

        m_rotationAxis.y = 1;
        m_rotationAxis.x = 0;
        m_rotationAxis.z = 0;

        m_TargetRotation = transform.rotation.eulerAngles;
        m_StartRotation = m_TargetRotation;

        m_MeshOffsetRotation = mesh.transform.localRotation.eulerAngles;

        OxyageAv.Add(1.0f);
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GetOxygenValue() < 1.0f)
            OxyageAv.Add(GetOxygenValue() + 0.01f);

        if(OxyageAv.Count > OxygenSamples)
        {
            OxyageAv.RemoveAt(0);
        }

        GameObject.FindGameObjectWithTag("HUD Camera").GetComponent<HUDstats>().HUDOxygen = GetOxygenValue();

        //check the controller is there if it isn't already
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

        //update xinput controller states
        m_gpPrevState = m_gpState;
        m_gpState = GamePad.GetState(m_PlayerIndex);


        if (Input.GetKeyDown(KeyCode.Escape) || (m_gpPrevState.Buttons.Start == ButtonState.Released && m_gpState.Buttons.Start == ButtonState.Pressed))
        {
            GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().PauseGame();
        }



        if (m_bPlayerCanMove)
        {
            //get movement information
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
            isRunning = false;
            if(Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
            }

            if ((m_gpPrevState.Buttons.LeftShoulder == ButtonState.Released && m_gpState.Buttons.LeftShoulder == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                rotate = true;
                if (!rotate)
                {
                    m_StartRotation = transform.rotation.eulerAngles;
                    m_TargetRotation = m_StartRotation + (m_rotationAxis * 90);
                }
                else
                {
                    m_TargetRotation += (m_rotationAxis * 90);
                }
            }
            else if ((m_gpPrevState.Buttons.RightShoulder == ButtonState.Released && m_gpState.Buttons.RightShoulder == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                rotate = true;
                if (!rotate)
                {
                    m_StartRotation = transform.rotation.eulerAngles;
                    m_TargetRotation = m_StartRotation + (m_rotationAxis * -90);
                }
                else
                {
                    m_TargetRotation += (m_rotationAxis * -90);
                }

            }

            //do smooth step for the camera roation
            if (rotate)
            {
                Vector3 rotation = Vector3.SmoothDamp(m_StartRotation, m_TargetRotation,
                                                      ref Rotatevelocity, Roation_time);
                m_StartRotation = rotation;
                transform.rotation = Quaternion.Euler(rotation);

                if (isCloseTo(rotation, m_TargetRotation))
                {
                    rotate = false;
                }
            }		

            // get a direction vector of the front of the camera
            Vector3 Direction = Front.transform.position - transform.position;
            Direction.Normalize();

            //get a direction vector to the Right of the player
            Vector3 DirectionRight = new Vector3(Direction.z, Direction.y, -Direction.x);

            // get the direction of travel
            Vector3 TotalDirection = Direction * LeftStick_yAxis + DirectionRight * LeftStick_xAxis;
            TotalDirection.Normalize();

            TotalDirection *= Time.deltaTime;

            transform.position += (TotalDirection) * (isRunning ? RunSpeed_mulitplier : MoveSpeed_mulitplier);

            //rotate the player model
            if (TotalDirection.magnitude > 0)
            {



                float angle = Mathf.Atan2(Vector3.Dot(Vector3.up, 
                                                      Vector3.Cross(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(m_CharictorTotalRotation) ,Vector3.one) * Direction , 
                                                                    TotalDirection)),
                                          Vector3.Dot(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(m_CharictorTotalRotation), Vector3.one) * Direction, TotalDirection));

                Vector3 TargetAngle = m_CharictorTotalRotation + new Vector3(0, Mathf.Rad2Deg * angle, 0);


                m_CharictorTotalRotation = Vector3.SmoothDamp(m_CharictorTotalRotation,
                                                           TargetAngle,
                                                      ref CharRotateVelocity, Char_Rotation_Time);

                mesh.transform.localRotation = Quaternion.Euler(m_CharictorTotalRotation + m_MeshOffsetRotation);


                
            }
        }
	}

 private bool isCloseTo(Vector3 r1, Vector3 r2)
        {
            bool x = Mathf.Approximately(r1.x, r2.x);
            bool y = Mathf.Approximately(r1.y, r2.y);
            bool z = Mathf.Approximately(r1.z, r2.z);

            return x && y && z;
        }

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Event") && other.GetComponent<Event>() && m_bPlayerCanMove)
        {
            Vector3 EventDirection = Vector3.Normalize(other.gameObject.transform.position - transform.position);

            RaycastHit hit;

            Debug.DrawRay(transform.position, EventDirection,Color.red);

            Physics.Raycast(transform.position, Vector3.Normalize(EventDirection), out hit);

            Debug.Log(hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag.Equals("Event"))
            {
                GameObject.FindGameObjectWithTag("HUD Camera").GetComponent<HUDstats>().event_close = true;
                if (Input.GetKey(KeyCode.E) || Input.GetKeyDown(KeyCode.E))
                {
                    EnablePlayerMovement(false);
                    GameObject.FindGameObjectWithTag("HUD Camera").GetComponent<HUDstats>().event_close = false;
                    other.GetComponent<Event>().Activate();
                }
            }
            else
            {
                GameObject.FindGameObjectWithTag("HUD Camera").GetComponent<HUDstats>().event_close = false;
            }
        }

        if(other.tag.Equals("Oxygen"))
        {
            float Distance = Vector3.Magnitude(other.transform.position - transform.position);
            

            Distance = Distance / (other.GetComponent<SphereCollider>().radius + GetComponent<SphereCollider>().radius);

            if (Distance <= 1)
                OxyageAv.Add(Distance);

            if (OxyageAv.Count > OxygenSamples)
            {
                OxyageAv.RemoveAt(0);
            }

        }


    }

	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Door"))
		{
			other.GetComponent<DoorManager>().Open();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag.Equals("Event"))
		{
			GameObject.FindGameObjectWithTag("HUD Camera").GetComponent<HUDstats>().event_close = false;
		}
		else if(other.tag.Equals("Door"))
		{
			other.GetComponent<DoorManager>().Close();
		}
	}

    public void EnablePlayerMovement(bool bCanmove = true)
    {
        m_bPlayerCanMove = bCanmove;
        GetComponentInChildren<Camera_Movement>().Enable(bCanmove);
    }

    public void PauseGame(bool pause = true)
    {
        if(pause)
        {
            m_bWasActive = m_bPlayerCanMove;

            EnablePlayerMovement(false);
        }
        else
        {
            EnablePlayerMovement(m_bWasActive);

        }
    }

    public float GetOxygenValue()
    {
        float total = 0;
        foreach (var item in OxyageAv)
        {
            total += item;
        }

        total /= OxyageAv.Count;
        return total;
    }


}

using UnityEngine;
using System.Collections;

public class LoadManager : MonoBehaviour {

	private bool m_manager_ready = false, g_manager_ready = false;
	public string scene_to_load;

	void CheckReady()
	{
		if(m_manager_ready && g_manager_ready)
		{
			Application.LoadLevel(scene_to_load);
		}
	}

	public void mManagerReady() {m_manager_ready = true; CheckReady();}
	public void gManagerReady() {g_manager_ready = true; CheckReady();}
}

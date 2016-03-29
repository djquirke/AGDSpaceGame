using UnityEngine;
using System.Collections;

public class MenuSwitch : MonoBehaviour {

    private GameObject CurrentMenu = null;

    public GameObject Parentobject = null;
   

    public void SetMenu(Object newMenu)
    {

        if(CurrentMenu != null)
        {
            DestroyImmediate(CurrentMenu);
        }

        CurrentMenu = (GameObject)Instantiate(newMenu);
       
        CurrentMenu.transform.SetParent(Parentobject.transform,false);
    }


}

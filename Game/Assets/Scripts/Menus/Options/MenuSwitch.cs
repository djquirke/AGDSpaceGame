using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSwitch : MonoBehaviour {

    private GameObject CurrentMenu = null;
    public GameObject DefaultMenu  = null;

    public List<GameObject> OtherMenus = new List<GameObject>();

    void Start()
    {
        foreach (var item in OtherMenus)
        {
            item.SetActive(false);
        }
        SetMenu(DefaultMenu);

    }

    public void SetMenu(GameObject newMenu)
    {

        if(CurrentMenu != null)
        {
            CurrentMenu.SetActive(false);
        }

        CurrentMenu = newMenu;

        CurrentMenu.SetActive(true);
    }


}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonSelect : MonoBehaviour  {

    public List<GameObject> m_SelectableObjects = new List<GameObject>();

    void Start()
    {
        

    }

    void update()
    {
        bool found = false;
        if(EventSystem.current.IsPointerOverGameObject())
        {
            //EventSystem.current.currentInputModule
        }

        foreach (var item in m_SelectableObjects)
        {
             if(item.name == EventSystem.current.currentSelectedGameObject.name)
             {
                 found = true; 
                 break;
             }

        }
        if (!found) 
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.lastSelectedGameObject);
        }
        
    }

    public void OnPointerEnter(GameObject go)
    {
        EventSystem.current.SetSelectedGameObject(go);
    }
  
}

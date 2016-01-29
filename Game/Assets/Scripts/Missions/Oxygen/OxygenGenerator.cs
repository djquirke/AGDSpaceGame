using UnityEngine;
using System.Collections;

public class OxygenGenerator : MonoBehaviour
{

    bool active = true;
    public float generatorValue = 1.0f;
    GameObject targetCell;

    // Use this for initialization
    void Start()
    {
        targetCell = GameObject.FindGameObjectWithTag("Floor");
        foreach (var cell in GameObject.FindGameObjectsWithTag("Floor"))
        {
            if (Vector3.Distance(targetCell.transform.position, this.transform.position) > Vector3.Distance(cell.transform.position, this.transform.position))
            {
                targetCell = cell;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            targetCell.GetComponent<OxygenManager>().AddOxygen(generatorValue * Time.deltaTime);
        }
    }
}


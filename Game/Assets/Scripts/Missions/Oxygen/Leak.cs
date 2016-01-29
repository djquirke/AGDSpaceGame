using UnityEngine;
using System.Collections;

public class Leak : MonoBehaviour
{
    bool active = false;
    float leakValue = 1.0f;
    float fixAmount = 0.0f;
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
        this.renderer.enabled = false;
    }

    public void Activate()
    {
        active = true;
        this.renderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            targetCell.GetComponent<OxygenManager>().AddOxygen(-leakValue * Time.deltaTime);
        }
    }

    public void Repair(float amount)
    {
        if (active)
        {
            fixAmount += amount;
            leakValue = 1.0f - fixAmount;
        }
        if (fixAmount >= 1.0f)
        {
            active = false;
            this.renderer.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableObject : MonoBehaviour
{
    public Vector3 pos;
    public Vector3 angle;
    bool disabled;
    public Transform table;
    public GameObject leftH;
    public GameObject rightH;
    public abstract void HairTrigger();

    protected void SetTable()
    {
        table = GameObject.Find("Table").transform;
        leftH = GameObject.Find("Controller (left)");
        rightH = GameObject.Find("Controller (right)");
    }

    protected void ToTable()
    {
        if(!disabled && transform.position.y < table.position.y
            && leftH.GetComponent<ControllerGrab>().grabbingObject != gameObject && rightH.GetComponent<ControllerGrab>().grabbingObject != gameObject)
        {
            disabled = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;
        }
        else if(disabled && transform.position.y > table.position.y + 0.2f)
        {
            disabled = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Collider>().enabled = true;
        }
        if(disabled)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0.5f, 0));
        }
    }
}

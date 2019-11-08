using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableObject : MonoBehaviour
{
    public Vector3 pos;
    public Vector3 angle;
    bool disabled;
    public abstract void HairTrigger();

    protected void ToTable()
    {
        if(!disabled && transform.position.y < GameManager.inst.table.transform.position.y
            && GameManager.inst.leftH.GetComponent<ControllerGrab>().grabbingObject != gameObject && 
            GameManager.inst.rightH.GetComponent<ControllerGrab>().grabbingObject != gameObject)
        {
            disabled = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;
        }
        else if(disabled && transform.position.y > GameManager.inst.table.transform.position.y + 0.2f)
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

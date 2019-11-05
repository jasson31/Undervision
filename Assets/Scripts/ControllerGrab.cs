using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrab : MonoBehaviour
{
    public SteamVR_Input_Sources input;
    public SteamVR_Action_Boolean grabButton;
    public SteamVR_Action_Boolean hairButton;
    public GameObject conModel;
    public ControllerGrab otherHand;

    GameObject collidingObject;
    [HideInInspector]
    public GameObject grabbingObject;
    [SerializeField]
    List<GameObject> collidingCand;
    private void Start()
    {
        collidingCand = new List<GameObject>();
        collidingObject = null;
        grabbingObject = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!grabbingObject)
        { 
            if (other.tag.Contains("Grabbable"))
            {
                if(!collidingCand.Contains(other.gameObject))
                {
                    collidingCand.Add(other.gameObject);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!grabbingObject)
        {
            if (other.tag.Contains("Grabbable"))
            {
                if (collidingCand.Contains(other.gameObject))
                {
                    collidingCand.Remove(other.gameObject);
                }
            }
        }
    }
    public void DropObject()
    {
        if (grabbingObject)
        {
            grabbingObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbingObject = null;
            conModel.SetActive(true);
        }
    }
    private void Update()
    {
        if(!grabbingObject && collidingCand.Count > 0)
        {
            collidingObject = collidingCand[0];
        }
        if(grabButton.GetLastStateDown(input))
        {
            if(!grabbingObject && collidingObject)
            {
                grabbingObject = collidingObject;
                grabbingObject.GetComponent<Rigidbody>().isKinematic = true;
                if (otherHand.grabbingObject == grabbingObject) otherHand.DropObject();
                collidingObject = null;
                collidingCand.Clear();
                conModel.SetActive(false);
            }
            else
            {
                DropObject();
            }
        }
        if(grabbingObject)
        {
            grabbingObject.transform.position = transform.position + grabbingObject.GetComponent<GrabbableObject>().pos;
            grabbingObject.transform.rotation = transform.rotation;
            grabbingObject.transform.Rotate(grabbingObject.GetComponent<GrabbableObject>().angle);
            if(hairButton.GetLastStateDown(input))
            {
                grabbingObject.GetComponent<GrabbableObject>().HairTrigger();
                if (grabbingObject.layer.Equals(LayerMask.NameToLayer("Panel")))
                {
                    grabbingObject = null;
                    conModel.SetActive(true);
                }
            }
        }
    }
}

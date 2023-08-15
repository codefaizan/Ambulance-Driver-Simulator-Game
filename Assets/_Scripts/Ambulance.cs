using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambulance : MonoBehaviour
{
    public Transform characterTargetPos;
    public Transform npcParent;
    public static bool isReached;
    public static string currentStopPoint;
    public static Ambulance i;

    private void Awake()
    {
        i = this;
    }

    internal void ToggleVehicleKinematicState()
    {
        GetComponent<RCC_CarControllerV3>().canControl = !GetComponent<RCC_CarControllerV3>().canControl;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = !this.gameObject.GetComponent<Rigidbody>().isKinematic;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("pickup point"))
        {
            UIController.i.ActivateOpenDoorButton(true);
            Destroy(other.gameObject);
            currentStopPoint = other.tag;
        }
        else if(other.CompareTag("drop point"))
        {
            if(GameController.i.passengers.Count > 0)
            {
            UIController.i.ActivateOpenDoorButton(true);
            currentStopPoint = other.tag;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("drop point"))
        {
            if (GameController.i.passengers.Count > 0)
            {
                UIController.i.ActivateOpenDoorButton(true);
                currentStopPoint = other.tag;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("drop point"))
        {
            UIController.i.ActivateOpenDoorButton(false);
            currentStopPoint = null;
        }
        else if(other.CompareTag("pickup point"))
        {
            currentStopPoint = null;
        }
    }
}

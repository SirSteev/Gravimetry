using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampCheck : MonoBehaviour
{
    List<Collider> rampTriggerList;
    public PlayerMovement playerMovment;

    public void Start()
    {
        rampTriggerList = new List<Collider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        rampTriggerList.Add(other);
        playerMovment.isRamp = true;
    }

    public void OnTriggerExit(Collider other)
    {
        rampTriggerList.Remove(other);
        
        if (rampTriggerList.Count == 0)
        {
            playerMovment.isRamp = false;
        }
    }
}

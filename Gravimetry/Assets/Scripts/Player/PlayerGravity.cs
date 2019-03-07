using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGravity : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerMouseLook playerMouseLook;
    public Rigidbody rigbod;
    public Gravity gravity;
    public PlayerScriptDebugs debugStuff;
    public GameObject playerBodyShell;
    public GameObject playerEyes;
    
    public GameObject masterForwardObject;
    public GameObject masterRightObject;
    public GameObject masterUpObject;

    [HideInInspector]
    public Vector3 playerForward;
    [HideInInspector]
    public Vector3 playerUp;
    [HideInInspector]
    public Vector3 playerRight;

    //[HideInInspector]
    public bool isRotating;
    
    void Start()
    {
        gravity.PreviousFallDirection = FallDirection.None;
        gravity.FallDirection = FallDirection.None;
        UpdateGravity(gravity.StartingFallDirection);
        isRotating = true;
    }
    
    void Update()
    {
        if (!playerMovement.isGrounded) rigbod.AddForce(gravity.GetForce());

        playerForward = masterForwardObject.transform.position - playerBodyShell.transform.position;
        playerForward = playerForward.normalized;
        playerRight = masterRightObject.transform.position - playerBodyShell.transform.position;
        playerRight = playerRight.normalized;
        playerUp = masterUpObject.transform.position - playerBodyShell.transform.position;
        playerUp = playerUp.normalized;


        float dot = Vector3.Dot(-gravity.GravityDirection, playerMovement.playerForward) / gravity.GravityDirection.magnitude;
        Vector3 newPos = playerMovement.playerForwardObject.transform.position + (gravity.GravityDirection * dot);
        Vector3 newForward = newPos - playerBodyShell.transform.position;
        newForward = newForward.normalized;


        if (debugStuff.debugPlayerGravity)
        {
            Debug.DrawRay(playerBodyShell.transform.position, newForward * 11000, Color.blue);
            Debug.DrawRay(playerBodyShell.transform.position, -gravity.GravityDirection * 11000, Color.green);
            Debug.DrawRay(playerBodyShell.transform.position, playerMovement.playerRight * 11000, Color.red);
            Debug.DrawRay(playerBodyShell.transform.position, playerMovement.playerForward * 11000, Color.cyan);
            Debug.DrawRay(playerMovement.playerForwardObject.transform.position, -gravity.GravityDirection * dot, Color.yellow);
        }

        if (rigbod.velocity.magnitude > 0.1 && !playerMovement.isGrounded)
        {
            

            Quaternion dirQ = Quaternion.LookRotation(newForward, -gravity.GravityDirection); // what direction i want
            
            Quaternion slerp = Quaternion.Slerp(playerBodyShell.transform.rotation, dirQ, rigbod.velocity.magnitude / gravity.RotationSpeed * Time.deltaTime); // rotates to it over time

            if (debugStuff.debugPlayerGravity)
            {
                Debug.Log("Forward: " + newForward + " / Up: " + -gravity.GravityDirection);
                Debug.Log("dirQ: " + dirQ.eulerAngles);
                Debug.Log("PlayerBody Rotation: " + playerBodyShell.transform.rotation.eulerAngles + " / slerp: " + slerp.eulerAngles);
            }
            
            playerBodyShell.transform.rotation = slerp;
        }
    }
    
    public void UpdateGravity(FallDirection _direction)
    {
        if (gravity.FallDirection == _direction) return;

        gravity.PreviousFallDirection = gravity.FallDirection;
        gravity.FallDirection = _direction;

        playerMovement.isGrounded = false;
        rigbod.constraints = RigidbodyConstraints.None;
        switch (gravity.FallDirection)
        {
            case FallDirection.XPos:
                gravity.GravityDirection = Vector3.right;
                break;
            case FallDirection.XNeg:
                gravity.GravityDirection = Vector3.left;
                break;
            case FallDirection.YPos:
                gravity.GravityDirection = Vector3.up;
                break;
            case FallDirection.YNeg:
                gravity.GravityDirection = Vector3.down;
                break;
            case FallDirection.ZPos:
                gravity.GravityDirection = Vector3.forward;
                break;
            case FallDirection.ZNeg:
                gravity.GravityDirection = Vector3.back;
                break;
            default:
                break;
        }

        rigbod.AddForce(gravity.GetForce());
        
    }
}

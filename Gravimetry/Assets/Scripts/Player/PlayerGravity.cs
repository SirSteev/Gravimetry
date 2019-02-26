using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGravity : MonoBehaviour
{
    public PlayerMovement playerMovment;
    public PlayerMouseLook playerMouseLook;
    public Rigidbody rigbod;
    public Gravity gravity;
    public BoolVariable debugStuff;
    public GameObject playerBody;
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
    
    void Start()
    {
        gravity.PreviousFallDirection = FallDirection.None;
        gravity.FallDirection = FallDirection.None;
        UpdateGravity(gravity.StartingFallDirection);
    }
    
    void Update()
    {
        if (!playerMovment.isGrounded) rigbod.AddForce(gravity.GetForce());

        playerForward = masterForwardObject.transform.position - playerBody.transform.position;
        playerForward = playerForward.normalized;
        playerRight = masterRightObject.transform.position - playerBody.transform.position;
        playerRight = playerRight.normalized;
        playerUp = masterUpObject.transform.position - playerBody.transform.position;
        playerUp = playerUp.normalized;

        if (debugStuff.Value)
        {
            Debug.DrawRay(playerBody.transform.position, playerForward * 11000, Color.blue);
            Debug.DrawRay(playerBody.transform.position, playerUp * 11000, Color.green);
            Debug.DrawRay(playerBody.transform.position, playerRight * 11000, Color.red);
        }

        Debug.DrawRay(playerBody.transform.position, Vector3.up * 11000, Color.green);
        Debug.DrawRay(playerBody.transform.position, -gravity.GravityDirection * 11000, Color.red);

        if (rigbod.velocity.magnitude > 0.1 && !playerMovment.isGrounded)
        {
            Quaternion dirQ = Quaternion.LookRotation(playerForward, -gravity.GravityDirection);
            if (gravity.FallDirection == FallDirection.ZPos || gravity.FallDirection == FallDirection.ZNeg)
                dirQ = Quaternion.LookRotation(Vector3.up, -gravity.GravityDirection);

            //-------------------------------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------------------------------

            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, rigbod.velocity.magnitude / gravity.RotationSpeed * Time.deltaTime);

            rigbod.MoveRotation(slerp);
        }
    }
    
    public void UpdateGravity(FallDirection _direction)
    {
        if (gravity.FallDirection == _direction) return;

        gravity.PreviousFallDirection = gravity.FallDirection;
        gravity.FallDirection = _direction;

        playerMovment.isGrounded = false;
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

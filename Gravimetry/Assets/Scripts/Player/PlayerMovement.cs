using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerGravity playerGravity;
    public PlayerMouseLook playerMouseLook;
    public Rigidbody rigbod;
    public Gravity gravity;
    public BoolVariable debugStuff;

    public PlayerStats playerStats;
    
    public GameObject playerBody;
    public GameObject playerEyes;

    public GameObject playerForwardObject;
    public GameObject playerRightObject;
    public GameObject playerUpObject;

    [HideInInspector]
    public Vector3 playerForward;
    [HideInInspector]
    public Vector3 playerUp;
    [HideInInspector]
    public Vector3 playerRight;

    bool moving = false;
    bool jumping = false;
    [HideInInspector]
    public bool isRamp;
    bool running = false;
    [HideInInspector]
    public bool isGrounded = false;

    List<Collider> feetTriggerList;

    private void Start()
    {
        feetTriggerList = new List<Collider>();
    }

    void Update()
    {
        playerForward = playerForwardObject.transform.position - playerBody.transform.position;
        playerForward = playerForward.normalized;
        playerRight = playerRightObject.transform.position - playerBody.transform.position;
        playerRight = playerRight.normalized;
        playerUp = playerUpObject.transform.position - playerBody.transform.position;
        playerUp = playerUp.normalized;

        if (debugStuff.Value)
        {
            Debug.DrawRay(playerBody.transform.position, playerForward * 10, Color.blue);
            Debug.DrawRay(playerBody.transform.position, playerUp * 10, Color.green);
            Debug.DrawRay(playerBody.transform.position, playerRight * 10, Color.red);
        }

        if (isGrounded && !jumping)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!running) running = true;
            }
            else
            {
                if (running) running = false;
            }

            if (Input.GetKey(KeyCode.W))
            {
                if (running) rigbod.velocity = playerForward * playerStats.MovmentSpeed * playerStats.RunMultiplier;
                else rigbod.velocity = playerForward * playerStats.MovmentSpeed;
                moving = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (running) rigbod.velocity = -playerRight * playerStats.MovmentSpeed * playerStats.RunMultiplier;
                else rigbod.velocity = -playerRight * playerStats.MovmentSpeed;
                moving = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (running) rigbod.velocity = -playerForward * playerStats.MovmentSpeed * playerStats.RunMultiplier;
                else rigbod.velocity = -playerForward * playerStats.MovmentSpeed;
                moving = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                if (running) rigbod.velocity = playerRight * playerStats.MovmentSpeed * playerStats.RunMultiplier;
                else rigbod.velocity = playerRight * playerStats.MovmentSpeed;
                moving = true;
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && !isRamp)
            {
                rigbod.AddForce(-gravity.GravityDirection * playerStats.JumpStrength);
                jumping = true;
                moving = true;
            }
            
            if (!moving && !jumping)
            {
                rigbod.velocity -= rigbod.velocity / 2;
                if (debugStuff.Value) Debug.Log("slowing player: PlayerMovment.cs");
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            playerMouseLook.ChangeGravity();
        }

        if (moving) moving = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        feetTriggerList.Add(other);

        isGrounded = true;
        jumping = false;

        rigbod.constraints = RigidbodyConstraints.FreezeRotation;

        transform.rotation = Quaternion.LookRotation(playerGravity.playerForward, -gravity.GravityDirection);

        //switch (gravity.FallDirection)
        //{
        //    case FallDirection.XPos:
        //        transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(playerForward, -gravity.GravityDirection));
        //        break;
        //    case FallDirection.XNeg:
        //        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, -90));
        //        break;
        //    case FallDirection.YPos:
        //        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(180, 0, 0));
        //        break;
        //    case FallDirection.YNeg:
        //        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
        //        break;
        //    case FallDirection.ZPos:
        //        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(-90, 0, 0));
        //        break;
        //    case FallDirection.ZNeg:
        //        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(90, 0, 0));
        //        break;
        //    default:
        //        break;
        //}
    }

    public void OnTriggerStay(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {
        feetTriggerList.Remove(other);
        
        if (feetTriggerList.Count == 0)
        {
            isGrounded = false;
        }
    }
}

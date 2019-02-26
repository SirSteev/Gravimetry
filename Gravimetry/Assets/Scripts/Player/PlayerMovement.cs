using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerGravity playerGravity;
    public PlayerMouseLook playerMouseLook;
    public Rigidbody rigbod;
    public Gravity gravity;
    public PlayerScriptDebugs debugStuff;

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

    RaycastHit hit;
    public GameObject masterDirectionHolder;

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

        if (!playerGravity.isRotating)
        {
            CheckRotation();
        }

        if (debugStuff.debugPlayerMovement)
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
            
            if (!moving && !jumping && rigbod.velocity.magnitude > 0.001)
            {
                rigbod.velocity -= rigbod.velocity * 0.9f;
                if (debugStuff.debugPlayerMovement) Debug.Log("slowing player: PlayerMovment.cs");
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) && !playerGravity.isRotating)
        {
            playerGravity.isRotating = true;
            playerMouseLook.ChangeGravity();
            CheckRotation(true);
        }

        if (moving) moving = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        feetTriggerList.Add(other);

        isGrounded = true;
        jumping = false;
        playerGravity.isRotating = false;

        rigbod.constraints = RigidbodyConstraints.FreezeRotation;

        playerBody.transform.rotation = Quaternion.LookRotation(playerForward, -gravity.GravityDirection);
        
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

    void CheckRotation(bool useGravity = false)
    {
        Physics.Raycast(playerBody.transform.position, playerForward, out hit, 11000f, LayerMask.GetMask("SkyBox"));
        
        if (useGravity)
        {
            switch (gravity.FallDirection)
            {
                case FallDirection.XPos:
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(gravity.GetPreviousFallDirectionVector(), Vector3.left);
                    break;
                case FallDirection.XNeg:
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(gravity.GetPreviousFallDirectionVector(), Vector3.right);
                    break;
                case FallDirection.YPos:
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(gravity.GetPreviousFallDirectionVector(), Vector3.down);
                    break;
                case FallDirection.YNeg:
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(gravity.GetPreviousFallDirectionVector(), Vector3.up);
                    break;
                case FallDirection.ZPos:
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(gravity.GetPreviousFallDirectionVector(), Vector3.back);
                    break;
                case FallDirection.ZNeg:
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(gravity.GetPreviousFallDirectionVector(), Vector3.forward);
                    break;
                case FallDirection.None:
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (hit.collider.gameObject.name)
            {
                case "XPos":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.right);
                    break;
                case "XNeg":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.left);
                    break;
                case "YPos":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.up);
                    break;
                case "YNeg":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.down);
                    break;
                case "ZPos":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                    break;
                case "ZNeg":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.back);
                    break;
                default:
                    Debug.Log("O SHIT I NEED AN ADULT: ray cast missed skybox player mouse look");
                    break;
            }
        }
    }
}

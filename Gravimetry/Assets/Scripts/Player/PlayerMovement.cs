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

    public GameObject playerMaster;
    public GameObject playerBody;
    public GameObject playerBodyShell;
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
    //[HideInInspector]
    public bool isRamp;
    bool running = false;
    [HideInInspector]
    public bool isGrounded = false;
    public bool isUpRight = false;

    List<Collider> feetTriggerList;

    private void Start()
    {
        feetTriggerList = new List<Collider>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        playerForward = playerForwardObject.transform.position - playerBody.transform.position;
        playerForward = playerForward.normalized;
        playerRight = playerRightObject.transform.position - playerBody.transform.position;
        playerRight = playerRight.normalized;
        playerUp = playerUpObject.transform.position - playerBody.transform.position;
        playerUp = playerUp.normalized;

        CheckRotation(!playerGravity.isRotating);

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
            rigbod.AddForce(gravity.GetPreviousFallDirectionVector() * playerStats.JumpStrength);
            isUpRight = false;
        }

        if (moving) moving = false;

        Vector3 f = playerBodyShell.transform.rotation.eulerAngles;
        Vector3 u = Quaternion.LookRotation(playerForward, -gravity.GravityDirection).eulerAngles;

        if (isGrounded && !isUpRight)
        {
            if (debugStuff.debugPlayerMovement) Debug.Log("grounded and not upright");

            float dot = Mathf.Abs(Vector3.Dot(-gravity.GravityDirection, playerForward) / -gravity.GravityDirection.magnitude);
            Vector3 newPos = playerForwardObject.transform.position + (-gravity.GravityDirection * dot);
            Vector3 newForward = newPos - playerBodyShell.transform.position;
            newForward = newForward.normalized;

            Quaternion dirQ = Quaternion.LookRotation(newForward, -gravity.GravityDirection); // what direction i want
            Quaternion slerp = Quaternion.Slerp(playerBodyShell.transform.rotation, dirQ, gravity.RotationSpeed * Time.deltaTime); // rotates to it over time


            if (dot > 0.001)
            {
                playerBodyShell.transform.rotation = slerp;
            }
            else
            {
                isUpRight = true;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        feetTriggerList.Add(other);

        isGrounded = true;
        jumping = false;
        playerGravity.isRotating = false;

        rigbod.constraints = RigidbodyConstraints.FreezeRotation;

        playerBody.transform.rotation = Quaternion.LookRotation(playerForward, -gravity.GravityDirection);
        playerMaster.transform.rotation = Quaternion.identity;
        
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
            masterDirectionHolder.transform.rotation = Quaternion.LookRotation(gravity.GetPreviousFallDirectionVector(), -gravity.GravityDirection);
        }
        else
        {
            switch (hit.collider.gameObject.name)
            {
                case "XPos":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.right, -gravity.GravityDirection);
                    break;
                case "XNeg":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.left, -gravity.GravityDirection);
                    break;
                case "YPos":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.up, -gravity.GravityDirection);
                    break;
                case "YNeg":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.down, -gravity.GravityDirection);
                    break;
                case "ZPos":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.forward, -gravity.GravityDirection);
                    break;
                case "ZNeg":
                    masterDirectionHolder.transform.rotation = Quaternion.LookRotation(Vector3.back, -gravity.GravityDirection);
                    break;
                default:
                    Debug.Log("O SHIT I NEED AN ADULT: ray cast missed skybox player mouse look");
                    break;
            }
        }
    }
}

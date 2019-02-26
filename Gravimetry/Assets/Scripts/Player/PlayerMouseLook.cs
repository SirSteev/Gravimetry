using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    public PlayerGravity playerGravity;
    public PlayerMovement playerMovment;
    public BoolVariable debugStuff;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    public GameObject playerBody;
    public GameObject playerEyes;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis
    public GameObject eyesForwardObject;
    public GameObject eyesRightObject;
    public GameObject eyesUpObject;

    Vector3 eyesForward;
    Vector3 eyesUp;
    Vector3 eyesRight;
    
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        eyesForward = eyesForwardObject.transform.position - playerEyes.transform.position;
        eyesForward = eyesForward.normalized;
        eyesRight = eyesRightObject.transform.position - playerEyes.transform.position;
        eyesRight = eyesRight.normalized;
        eyesUp = eyesUpObject.transform.position - playerEyes.transform.position;

        if (debugStuff.Value)
        {
            Debug.DrawRay(playerEyes.transform.position, eyesForward * 11000, Color.blue);
            Debug.DrawRay(playerEyes.transform.position, eyesUp * 11000, Color.green);
            Debug.DrawRay(playerEyes.transform.position, eyesRight * 11000, Color.red);
        }
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        playerEyes.transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        playerBody.transform.localRotation = Quaternion.Euler(0, rotY, 0);
    }

    public void ChangeGravity()
    {
        RaycastHit hit;
        Physics.Raycast(playerEyes.transform.position, eyesForward, out hit, 11000f, LayerMask.GetMask("SkyBox"));

        //Debug.Log("ray hit: " + hit.collider.gameObject.name);

        switch (hit.collider.gameObject.name)
        {
            case "XPos":
                playerGravity.UpdateGravity(FallDirection.XPos);
                break;
            case "XNeg":
                playerGravity.UpdateGravity(FallDirection.XNeg);
                break;
            case "YPos":
                playerGravity.UpdateGravity(FallDirection.YPos);
                break;
            case "YNeg":
                playerGravity.UpdateGravity(FallDirection.YNeg);
                break;
            case "ZPos":
                playerGravity.UpdateGravity(FallDirection.ZPos);
                break;
            case "ZNeg":
                playerGravity.UpdateGravity(FallDirection.ZNeg);
                break;
            default:
                Debug.Log("O SHIT I NEED AN ADULT: ray cast missed skybox player mouse look");
                break;
        }
    }
}

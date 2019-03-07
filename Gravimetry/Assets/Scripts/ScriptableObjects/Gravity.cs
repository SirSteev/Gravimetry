using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FallDirection
{
    XPos,
    XNeg,
    YPos,
    YNeg,
    ZPos,
    ZNeg,
    None
}

[CreateAssetMenu]
public class Gravity : ScriptableObject
{
    public string Desctiption;
    public FallDirection StartingFallDirection;
    public FallDirection FallDirection;
    public FallDirection PreviousFallDirection;
    public Vector3 GravityDirection;
    public float GravitySpeed;
    public float RotationSpeed;
    
    public Vector3 GetForce()
    {
        return GravityDirection * GravitySpeed;
    }

    public Vector3 GetPreviousFallDirectionVector()
    {
        Vector3 vector = Vector3.zero;

        switch (PreviousFallDirection)
        {
            case FallDirection.XPos:
                vector = Vector3.left;
                break;
            case FallDirection.XNeg:
                vector = Vector3.right;
                break;
            case FallDirection.YPos:
                vector = Vector3.down;
                break;
            case FallDirection.YNeg:
                vector = Vector3.up;
                break;
            case FallDirection.ZPos:
                vector = Vector3.back;
                break;
            case FallDirection.ZNeg:
                vector = Vector3.forward;
                break;
            case FallDirection.None:
                vector = Vector3.up;
                break;
            default:
                break;
        }

        return vector;
    }
}

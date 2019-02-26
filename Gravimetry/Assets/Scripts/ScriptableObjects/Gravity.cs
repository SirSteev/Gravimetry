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
}

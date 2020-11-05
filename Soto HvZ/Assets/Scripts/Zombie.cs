using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Vehicle
{
    public GameObject seekTarget;
    float seekWeight;
    void Start()
    {
        mass = 5;
        maxSpeed = 0.02f;
        base.Start();
    }
    public override void CalcSteeringForces()
    {
        Vector3 ultForce = new Vector3(0, 0, 0);
        ultForce += Seek(seekTarget);
        ultForce = Vector3.ClampMagnitude(ultForce, maxSpeed);
        acceleration += ultForce;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    public GameObject fleeTarget;
    public GameObject seekTarget;
    float fleeWeight;
    float seekWeight;

    void Start()
    {
        mass = 1;
        maxSpeed = 0.025f;
        base.Start();
    }
        
  
    public override void CalcSteeringForces()
    {
        Vector3 ultForce = new Vector3(0,0,0);
        Vector3 distance = seekTarget.transform.position - fleeTarget.transform.position;
        ultForce += Seek(seekTarget);
        if(distance.x <5 || distance.z <5)
        {
            ultForce += Flee(fleeTarget);
        }
        ultForce = Vector3.ClampMagnitude(ultForce, maxSpeed);
        acceleration += ultForce;
    }

}

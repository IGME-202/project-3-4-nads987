using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Vehicle
{
    public GameObject seekTarget;
    List<GameObject> humansList;
    public bool near;
    

    public override void Start()
    {
        base.Start();
        mass = 5;
        maxSpeed = 0.01f;
        gameObject.GetComponent<Zombie>().manager = GameObject.Find("Manager");

    }

    public override void CalcSteeringForces()
    {
        Vector3 ultForce = Vector3.zero;
        if (near == true)
        { ultForce += Pursue(seekTarget); }
        ultForce += ObstacleAvoidance();
        ultForce = Vector3.ClampMagnitude(ultForce, maxSpeed);
        ApplyForce(ultForce);

    }

}
    


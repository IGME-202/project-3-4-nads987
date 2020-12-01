
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    public GameObject fleeTarget;
    List<GameObject> zombiesList;
    Vector3 currDistance;
    Vector3 prevDistance;
    Vector3 distance;
   Vector3 ultForce;
    public bool near;
    
    public override void Start()
    {
        
        base.Start();
        mass = 1;
        maxSpeed = 0.018f;
        gameObject.GetComponent<Human>().manager = GameObject.Find("Manager");
    }


    public override void CalcSteeringForces()
    {
        Vector3 ultForce = Vector3.zero;
        if (near == true)
        { ultForce += Evade(fleeTarget); }
        ultForce += ObstacleAvoidance();
        ultForce = Vector3.ClampMagnitude(ultForce, maxSpeed);
        ApplyForce(ultForce);
    }

    }


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
   
    public override void Start()
    {
        base.Start();
        mass = 1;
        maxSpeed = 0.02f;
        gameObject.GetComponent<Human>().manager = GameObject.Find("Manager");
    }


    public override Vector3 CalcSteeringForces()
    {
        Vector3 ultForce = Vector3.zero;
        zombiesList = manager.GetComponent<Manager>().zombies;
        ultForce += ObstacleAvoidance();
        for (int i = 0; i < zombiesList.Count; i++)
        {
            currDistance = gameObject.transform.position - zombiesList[i].transform.position;

            if (currDistance.x < prevDistance.x || currDistance.z < prevDistance.z)
            {
                fleeTarget = zombiesList[i];
                distance = currDistance;
            }

            prevDistance = currDistance;
        }
        if (distance.x < 3 || distance.z < 3)
        {
            ultForce += Flee(fleeTarget);
            ultForce = Vector3.ClampMagnitude(ultForce, maxSpeed);
            
        }
        return ultForce;
    }

    }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Vehicle
{
    public GameObject manager;
    public GameObject seekTarget;
    List<GameObject> humansList;


    public override void Start()
    {
        base.Start();
        mass = 7;
        maxSpeed = 0.01f;
        gameObject.GetComponent<Zombie>().manager = GameObject.Find("Manager");

    }

    public override Vector3 CalcSteeringForces()
    {
        Vector3 ultForce = Vector3.zero;
        humansList = manager.GetComponent<Manager>().humans;

        //look thru the humans list
        for (int i = 0; i < humansList.Count; i++)
        {
            Vector3 distance = gameObject.transform.position - humansList[i].transform.position;

            if (distance.x < 5 || distance.z < 5)
            {
                seekTarget = humansList[i];
                ultForce += Seek(seekTarget);
                ultForce = Vector3.ClampMagnitude(ultForce, maxSpeed);

            }

        }
        return ultForce;
    }
}

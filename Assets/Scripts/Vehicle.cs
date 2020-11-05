using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    public float accMag;
    private Vector3 vehiclePos;
    public Vector3 velocity;
    public Vector3 direction = Vector3.up;
    public Vector3 acceleration;
    public float decelerationRate;
    public float maxSpeed;
    Quaternion angle;
    public float walkForce;
    public Vector3 gravForce = new Vector3(0f, -1f, 0f);
    public bool frictionOn = true;
    public float mass;
    Vector3 up = new Vector3(0, 0, 1);
    public float coefficent;
    public Vector3 force;
    // Start is called before the first frame update
    public void Start()
    {
        //vehiclePos = Vector3.zero;
        velocity = Vector3.zero;
        direction = Vector3.right;
        acceleration = Vector3.zero;
        decelerationRate = -0.1f;
        force = new Vector3(0.5f,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        CalcSteeringForces();
        UpdatePosition();
        WrapVehicle();
        SetTransform();
        ApplyForce(force);
    }
    void UpdatePosition()
    {
        velocity += acceleration;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        vehiclePos += velocity;
        vehiclePos.y = 1.5f;
        direction = velocity.normalized;
        velocity = Vector3.ClampMagnitude(velocity, 0f);
    }
    void SetTransform()
    {
        transform.position = vehiclePos;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
    void WrapVehicle()
    {
        if (vehiclePos.x >= 8.77)
        {
            vehiclePos.x = -8.77f+1;
        }
        else if (vehiclePos.x <= -8.77)
        {
            vehiclePos.x = 8.77f-1;
        }
        if (vehiclePos.z >= 8.77)
        {
            vehiclePos.z = -8.77f +1;
        }
        else if (vehiclePos.z <= -8.77)
        {
            vehiclePos.z = 8.77f-1;
        }


    }
    void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        acceleration += friction;

    }
    void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }
    public Vector3 Seek(GameObject target)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = target.transform.position - vehiclePos;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - velocity;

        // Return seek steering force
        return seekingForce;
    }
    public Vector3 Seek(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = targetPos - vehiclePos;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - velocity;

        // Return seek steering force
        return seekingForce;
    }
    public Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity = vehiclePos - targetPos;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        Vector3 fleeingForce = desiredVelocity - velocity;
        return fleeingForce;

    }
    public Vector3 Flee(GameObject target)
    {
        return Flee(target.transform.position);
    }
    public abstract void CalcSteeringForces();

    }

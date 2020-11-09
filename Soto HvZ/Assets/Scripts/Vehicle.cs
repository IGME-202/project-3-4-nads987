using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    public Vector3 vehiclePos;
    Vector3 velocity;
    Vector3 direction;
    public Vector3 acceleration;
    public float maxSpeed;
    Quaternion angle;
    //Vector3 gravForce = new Vector3(0f, -0.003f, 0f);
    public bool frictionOn = true;
    public float mass;
    float coefficent;
    Vector3 force;
    public Material material1;
    public Material material2;

    // Start is called before the first frame update
   public virtual void Start()
   {
        vehiclePos = gameObject.transform.position;
        //velocity = Vector3.zero;
        direction = Vector3.up;
        acceleration = Vector3.zero;
       // decelerationRate = -0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        force = CalcSteeringForces();
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
        vehiclePos.y = 1.2f;
        direction = velocity.normalized;
        acceleration = Vector3.ClampMagnitude(acceleration, 0f);
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
            vehiclePos.x = 8.77f+1;
            Bounce(new Vector3(-1, 0, 0));
        }
        else if (vehiclePos.x <= -8.77)
        {
            vehiclePos.x = -8.77f-1;
            Bounce(new Vector3(1, 0, 0));
        }
        if (vehiclePos.z >= 8.77)
        {
            vehiclePos.z = 8.77f +1;
            Bounce(new Vector3(0, 0, -1));
        }
        else if (vehiclePos.z <= -8.77)
        {
            vehiclePos.z = -8.77f-1;
            Bounce(new Vector3(0, 0, 1));
        }


    }
    void Bounce(Vector3 normal)
    {
        velocity = Vector3.Reflect(velocity, normal);
        acceleration = Vector3.Reflect(acceleration, normal);

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
    public abstract Vector3 CalcSteeringForces();

    
    public void OnRenderObject() // Examples of drawing lines – yours might be more complex!
    {
        // Set the material to be used for the first line
        material1.SetPass(0);

        // Draws one line		
        GL.Begin(GL.LINES);                 // Begin to draw lines
        GL.Vertex(vehiclePos);        // First endpoint of this line
        GL.Vertex(gameObject.transform.forward);        // Second endpoint of this line
        //Debug.Log(gameObject.transform.forward);
        GL.End();                       // Finish drawing the line

        // Second line
        // Set another material to draw this second line in a different color
        material2.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(vehiclePos);
        GL.Vertex(gameObject.transform.right);
        //Debug.Log(gameObject.transform.right);
        GL.End();
    }

}

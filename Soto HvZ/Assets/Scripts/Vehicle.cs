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
    //forces
    public bool frictionOn = true;
    public float mass;
    float coefficent;
    Vector3 force;
   //gizmos
    public Material material1;
    public Material material2;
    public Mesh boxMesh;
    public GameObject manager;
    //obstacle avoidance

    public float radius = 3f;
    public float avoidanceRange = 3f;
    List<Obstacle> obstacles;
    // Start is called before the first frame update
    public virtual void Start()
    {
        vehiclePos = gameObject.transform.position;
        velocity = Vector3.zero;
        direction = Vector3.up;
        
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

    //keep the vehicle within bounds
    void WrapVehicle()
    {
        if (vehiclePos.x >= 9f)
        {
            vehiclePos.x = 9f-1f;
            Bounce(new Vector3(-2, 0, 0));
        }
        else if (vehiclePos.x <= -9f)
        {
            vehiclePos.x = -9f+1f;
            Bounce(new Vector3(2, 0, 0));
        }
        if (vehiclePos.z >= 9.5f)
        {
            vehiclePos.z = 9.5f -1f;
            Bounce(new Vector3(0, 0, -2));
        }
        else if (vehiclePos.z <= -9.5f)
        {
            vehiclePos.z = -9.5f+1f;
            Bounce(new Vector3(0, 0, 2));
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

    protected Vector3 ObstacleAvoidance()
    {
       obstacles = manager.GetComponent<Manager>().obstacles;
        Vector3 right = Vector3.Cross(velocity, Vector3.up);
        Vector3 avoidanceSteering = Vector3.zero;
        float dotProduct;
        Vector3 toOther;
        
        foreach (Obstacle other in obstacles)
        {
            toOther = other.transform.position - transform.position;
            dotProduct = Vector3.Dot(velocity, toOther);

            if( dotProduct >= 0)
            {
                if(Vector3.Distance(transform.position,other.transform.position) < avoidanceRange + other.radius)
                {
                    dotProduct = Vector3.Dot(right, toOther);

                    if(Mathf.Abs(dotProduct) <= radius + other.radius)
                    {
                        if(dotProduct>= 0)
                        {
                            avoidanceSteering += -right.normalized * maxSpeed;
                        }
                        else
                        {
                            avoidanceSteering += right.normalized * maxSpeed;
                        }
                    }
                }
            }
        }


        return avoidanceSteering;
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, avoidanceRange);

        //draw future pos
        Gizmos.color = Color.green;
        Vector3 futurePos = transform.position + velocity;
        Gizmos.DrawWireSphere(futurePos, radius);
    }
    public void OnRenderObject() // Examples of drawing lines – yours might be more complex!
    {
        // Set the material to be used for the first line
        material1.SetPass(0);

        // Draws one line		
        GL.Begin(GL.LINES);                 // Begin to draw lines
        GL.Vertex(transform.position);        // First endpoint of this line
        GL.Vertex(gameObject.transform.forward);        // Second endpoint of this line
        //Debug.Log(gameObject.transform.forward);
        GL.End();                       // Finish drawing the line

        // Second line
        // Set another material to draw this second line in a different color
        material2.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(transform.position);
        GL.Vertex(gameObject.transform.right);
        //Debug.Log(gameObject.transform.right);
        GL.End();
    }

}

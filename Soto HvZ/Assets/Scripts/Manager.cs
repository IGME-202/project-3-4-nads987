using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{   //game object lists
    public List<GameObject> humans;
    public List<GameObject> zombies;
    public List<GameObject> entities;
    public List<Obstacle> obstacles;
    public Button humanButton;
    public Button zombieButton;
     Vehicle vehicleObj;
    GameObject seekTarget;
    GameObject fleeTarget;
    public Vector3 distance;
    //prefabs
    public GameObject humanPrefab;
    public GameObject zombiePrefab;
    public Obstacle obstaclePrefab;
    int humansAmt = 4;
    int zombiesAmt = 2;
    int objectsAmt = 6;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
        MakeEntityList();
        Button humButton = humanButton.GetComponent<Button>();
        humButton.onClick.AddListener(AddHuman);
        Button zomButton = zombieButton.GetComponent<Button>();
        zomButton.onClick.AddListener(AddZombie);



    }

    // Update is called once per frame
    void Update()
    {
        WanderingCharacters();
        for (int i = 0; i < zombies.Count; i++)
        {
            if (i > 0)
            { vehicleObj.ApplyForce(vehicleObj.Seperate(zombies[i - 1])); }
        }
       
        if (humans.Count > 0)
        {
            for (int i = 0; i < humans.Count; i++)
            {
                if (i > 0)
                { vehicleObj.ApplyForce(vehicleObj.Seperate(humans[i - 1])); }
            }
            for (int j = 0; j < zombies.Count; j++)
            {
                for (int i = 0; i < humans.Count; i++)
            {
              
                    if (CheckForCollision(humans[i].GetComponent<BoxCollider>(), zombies[j].GetComponent<BoxCollider>()))
                    {
                        Debug.Log("hit");
                        zombies.Add(Instantiate(zombiePrefab, humans[i].transform.position, Quaternion.identity));
                        Destroy(humans[i]);
                        humans.Remove(humans[i]);

                    }
                }
                
            }
        }
        if(humans.Count == 0)
        {
            for (int i = 0; i < zombies.Count; i++)
            {
                //Debug.Log("All humans caught");
                vehicleObj = GameObject.FindObjectOfType(typeof(Vehicle)) as Vehicle;
                vehicleObj.ApplyForce(vehicleObj.Wander(zombies[i]));
                zombies[i].GetComponent<Zombie>().near = false;
                zombies[i].GetComponent<Zombie>().seekTarget = null;

            }
            
        }
        

    }

    void AddHuman()
    {
        for (int i = 0; i < 1; i++)
        {
            float randX = Random.Range(-7.50f, 7.50f);
            float randZ = Random.Range(-7.50f, 7.50f);
            Vector3 pos = new Vector3(randX, 0.25f, randZ);
            humans.Add(Instantiate(humanPrefab, pos, Quaternion.identity));

        }

    }
    void WanderingCharacters()
    {
        for (int j = 0; j < zombies.Count; j++)
        {
            //look thru the humans list
            for (int i = 0; i < humans.Count; i++)
            {
                distance = zombies[j].transform.position - humans[i].transform.position;

                if (distance.z < 3 || distance.x < 3)
                {
                    
                    seekTarget = humans[i];
                    fleeTarget = zombies[j];

                    humans[i].GetComponent<Human>().fleeTarget = fleeTarget;
                    zombies[j].GetComponent<Zombie>().seekTarget = seekTarget;
                    humans[i].GetComponent<Human>().near = true;
                    zombies[j].GetComponent<Zombie>().near = true;

                }

                else
                {
                   
                    vehicleObj = GameObject.FindObjectOfType(typeof(Vehicle)) as Vehicle;
                    vehicleObj.ApplyForce(vehicleObj.Wander(humans[i]));
                    vehicleObj.ApplyForce(vehicleObj.Wander(zombies[j]));
                    vehicleObj.ApplyForce(vehicleObj.Seperate(humans[i]));
                    vehicleObj.ApplyForce(vehicleObj.Seperate(zombies[j]));
                    humans[i].GetComponent<Human>().near = false;
                    zombies[j].GetComponent<Zombie>().near = false;

                   // Debug.Log("Wandering");

                }
            }
        }
    }
    void AddZombie()
    {
        for (int i = 0; i < 1; i++)
        {
            float randX = Random.Range(-7.50f, 7.50f);
            float randZ = Random.Range(-7.50f, 7.50f);
            Vector3 pos = new Vector3(randX, 0.25f, randZ);
            zombies.Add(Instantiate(zombiePrefab, pos, Quaternion.identity));
        }
    }
    
    //check if human and zombies are colliding
    bool CheckForCollision(BoxCollider objA, BoxCollider objB)
    {
        bool isHitting = false;
        if (objB.bounds.min.x < objA.bounds.max.x &&
                    objB.bounds.max.x > objA.bounds.min.x &&
                    objB.bounds.max.z < objA.bounds.min.z &&
                    objB.bounds.min.z < objA.bounds.max.z)
        {
            isHitting = true;
        }
        return isHitting;

    } 
    //spawn alll the game objects
    void Spawn()
    {
        //spawn humans
        for (int i = 0; i < humansAmt; i++)
        {
           Vector3 pos = new Vector3(0, 0.25f, 0);
           humans.Add(Instantiate(humanPrefab, pos, Quaternion.identity));

        }
        //spawn zombies
        for (int i = 0; i < zombiesAmt; i++)
        {
            Vector3 pos = new Vector3(0, 0.25f, 0);
            zombies.Add(Instantiate(zombiePrefab, pos, Quaternion.identity));

        }
        //create objects + change positions to random
        for (int i = 0; i < objectsAmt; i++)
        {
            float randX = Random.Range(-13.40f, 13.40f);
            float randZ = Random.Range(-13.40f, 13.40f);
            Vector3 pos = new Vector3(randX, -0.1f, randZ);
            obstacles.Add(Instantiate(obstaclePrefab, pos, Quaternion.identity));
            
        }
    }
    //add humans and zombies to entities list and change pos
    void MakeEntityList()
    {
        for (int i = 0; i < humans.Count; i++)
        {
           entities.Add(humans[i]);
        }
        for (int i = 0; i < zombies.Count; i++)
        {

            entities.Add(zombies[i]);

        }
        for (int i = 0; i < entities.Count; i++)
        {
            float randX = Random.Range(-7.50f, 7.50f);
            float randZ = Random.Range(-7.50f, 7.50f);
            Vector3 pos = new Vector3(randX, entities[i].transform.position.y, randZ);
           entities[i].transform.position = pos;
        }
    }
}

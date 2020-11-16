using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Manager : MonoBehaviour
{   //game object lists
    public List<GameObject> humans;
    public List<GameObject> zombies;
    public List<GameObject> entities;
    public List<Obstacle> obstacles;

    //prefabs
    public GameObject humanPrefab;
    public GameObject zombiePrefab;
    public Obstacle obstaclePrefab;
   
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
        MakeEntityList();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < humans.Count; i++)
        {
            for (int j = 0; j < zombies.Count; j++)
            {
                if (CheckForCollision(humans[i].GetComponent<BoxCollider>(),zombies[j].GetComponent<BoxCollider>()))
                {
                    Debug.Log("hit");
                    zombies.Add(Instantiate(zombiePrefab, humans[i].transform.position, Quaternion.identity));
                    Destroy(humans[i]);
                    humans.Remove(humans[i]);
                    
                }
                
            }
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
        for (int i = 0; i < 4; i++)
        {
           Vector3 pos = new Vector3(0, 1f, 0);
           humans.Add(Instantiate(humanPrefab, pos, Quaternion.identity));

        }
        //spawn zombies
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = new Vector3(0, 1.5f, 0);
            zombies.Add(Instantiate(zombiePrefab, pos, Quaternion.identity));

        }
        //create objects + change positions to random
        for (int i = 0; i < 8; i++)
        {
            float randX = Random.Range(-9.40f, 9.40f);
            float randZ = Random.Range(-9.40f, 9.40f);
            Vector3 pos = new Vector3(randX, 1f, randZ);
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
            float randX = Random.Range(-9.40f, 9.40f);
            float randZ = Random.Range(-9.40f, 9.40f);
            Vector3 pos = new Vector3(randX, entities[i].transform.position.y, randZ);
            entities[i].transform.position = pos;
        }
    }
}

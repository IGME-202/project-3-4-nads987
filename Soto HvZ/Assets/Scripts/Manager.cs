using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Manager : MonoBehaviour
{
    public List<GameObject> humans;
    public List<GameObject> zombies;
    public List<GameObject> entities;
    public GameObject humanPrefab;
    public GameObject zombiePrefab;
    Vehicle vScript;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
        MakeEntityList();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //obj a=human obj b= zombie
    bool CheckForCollision(BoxCollider objA, BoxCollider objB)
    {
        bool isHitting = false;

        if (objB.bounds.min.x < objA.bounds.max.x &&
                   objB.bounds.max.x > objA.bounds.min.x &&
                   objB.bounds.max.y < objA.bounds.min.y &&
                   objB.bounds.min.y < objA.bounds.max.y)
        {
            isHitting = true;

        }
        return isHitting;
    }
    
    void Spawn()
    {
        for (int i = 0; i < 3; i++)
        {
           
           Vector3 pos = new Vector3(0, 1f, 0);
           humans.Add(Instantiate(humanPrefab, pos, Quaternion.identity));

        }
        for (int i = 0; i < 3; i++)
        {
          
            Vector3 pos = new Vector3(0, 1.5f, 0);
            zombies.Add(Instantiate(zombiePrefab, pos, Quaternion.identity));

        }
    }
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

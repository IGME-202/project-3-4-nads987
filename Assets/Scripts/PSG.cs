using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSG : MonoBehaviour
{
    public GameObject human;
    Vector3 distance;
    Vector3 psgPos;
    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.zero;
        psgPos = Vector3.zero;
        psgPos.y = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {

        distance = psgPos - human.transform.position;
        if (distance.x < 0.5 || distance.z < 0.5)
        {
            float randomX = Random.Range(-8, 8);
            float randomZ = Random.Range(-8, 8);
            psgPos = new Vector3(randomX, 1.5f, randomZ);

        }
        gameObject.transform.position = psgPos;
    }
}

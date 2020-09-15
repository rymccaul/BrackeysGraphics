using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle_spawner : MonoBehaviour
{
    public GameObject obstacle;
    public int traffic_count;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < traffic_count; i++)
        {
            Instantiate(obstacle, new Vector3(0, 0, -(i + 100)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

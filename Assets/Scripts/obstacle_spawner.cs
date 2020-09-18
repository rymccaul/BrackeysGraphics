using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle_spawner : MonoBehaviour
{
    /*
    Prefab array holds prefabs of NPC cars with their children uv-maps and
    sensors, and new ones must be created in order to add more models or models
    with different colors/materials.
    */
    public GameObject[] prefab_array;
    public int traffic_count;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < traffic_count; i++)
        {
            Instantiate(prefab_array[Random.Range(0,prefab_array.Length)],
                            new Vector3(0, 0, -(i + 100)), Quaternion.identity);
        }
    }
}

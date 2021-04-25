using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building_spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bldgRootPrefab;
    public int bldgRowsCount;

    public ver2_bldg_movement bldgMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        
        bldgRowsCount = 6;
        float roadDivision = 600 / (bldgRowsCount + 1);


        for (int i = -1; i < bldgRowsCount; i++)
        {
            //int randomSign = Random.Range(0, 2);
            //float randomX = Random.Range(15f, 25f) * ((randomSign - 0.5f) * 2);
            //float Xpos = -76.2f;

            float randomZ = (i * roadDivision) + roadDivision;
            //randomZ += (Random.Range(-30f, 30f));


            //Vector3 scale = new Vector3(30, 30 * randomSign, 30);

            Instantiate(bldgRootPrefab, new Vector3(-33.5f, -5f, randomZ), Quaternion.identity);

        }

        //Instantiate(bldgRootPrefab, new Vector3(-52, -5, -2), bldgRotation);
        
    }
}

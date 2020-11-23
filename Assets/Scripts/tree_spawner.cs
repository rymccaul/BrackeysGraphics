using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tree_spawner : MonoBehaviour
{

    public GameObject[] tree_prefab_array;
    public int treeCount;
    // Start is called before the first frame update
    void Start()
    {

        float roadDivision = 500 / (treeCount + 1);

        for (int i = 0; i < treeCount; i++)
        {
            int randomSign = Random.Range(0, 2);
            float randomX = Random.Range(15f, 25f) * ((randomSign - 0.5f) * 2);

            float randomZ = (i * roadDivision) + roadDivision;
            randomZ += (Random.Range(-30f, 30f));

            Quaternion treeRotation = Quaternion.Euler(-90, 0, 0);

            Instantiate(tree_prefab_array[Random.Range(0, tree_prefab_array.Length)],
                            new Vector3(randomX, -1, randomZ), treeRotation);
        }
    }

}

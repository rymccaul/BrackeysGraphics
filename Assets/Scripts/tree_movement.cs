using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tree_movement : MonoBehaviour
{
    public GameObject tree;
    public GameObject player;
    private Vector3 treeSpeed;
    private Vector3 treePos;
    private float playerZpos;

    // Start is called before the first frame update
    void Start()
    {
        treeSpeed = new Vector3(0, 0, -50f);
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        treePos = transform.position;
        treePos += (treeSpeed * Time.deltaTime);

        transform.position = treePos;
        playerZpos = player.transform.position.z;

        if (treePos.z < (playerZpos - 10))
        {
            Respawn(playerZpos);
        }
    }

    void Respawn(float playerPosition)
    {
        int randomSign = Random.Range(0, 2);
        float randomX = Random.Range(15f, 25f) * ((randomSign - 0.5f) * 2);

        transform.position = new Vector3(randomX, -1, 500 + playerPosition);
    }
}

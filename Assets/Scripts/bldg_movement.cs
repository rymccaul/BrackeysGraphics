using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bldg_movement : MonoBehaviour
{
    public GameObject player;
    private Vector3 bldgSpeed;
    private Vector3 bldgPos;
    private float playerZpos;
    private bool actuallyMoving;
    public float scaleIncrease;
    public float startingScale;
    public float posIncrease;

    //public int imageFlipped;

    public float defaultXpos;
    public float respawnXpos;

    private float playerSpeedFactor;
    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        bldgSpeed = new Vector3(0, 0, -25f);
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();

        transform.localScale = new Vector3(30, 30, 30);
        defaultXpos = -33.5f;
        respawnXpos = -58.8f;
        actuallyMoving = true;
        scaleIncrease = 1f;
        startingScale = 0.5f;
        posIncrease = scaleIncrease / (30 * (1 - startingScale));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (actuallyMoving == true)
        {

            bldgPos = transform.position;
            bldgPos += (bldgSpeed * Time.deltaTime);

            transform.position = bldgPos;
            playerZpos = player.transform.position.z;

            if (bldgPos.z < (playerZpos - 10))
            {
                Respawn(playerZpos);
            }
        }
        else
        {
            Vector3 currentScale = transform.localScale;
            playerSpeedFactor = 50 + playerRb.velocity.z;
            playerSpeedFactor /= 50;

            currentScale += new Vector3(scaleIncrease, scaleIncrease, scaleIncrease) * Time.deltaTime * playerSpeedFactor;
            bldgPos = transform.position;
            //bldgPos += (bldgSpeed * Time.deltaTime) * 0.5f;
            //transform.position = bldgPos;

            if (currentScale.x < 30)
            {
                transform.localScale = currentScale;
                bldgPos += (bldgSpeed * Time.deltaTime) * 0.5f;
                //bldgPos.x -= posIncrease;
            }
            else
            {
                currentScale = new Vector3(30, 30, 30);
                transform.localScale = currentScale;

                bldgPos += (bldgSpeed * Time.deltaTime);
                //bldgPos.x = defaultXpos;
                actuallyMoving = true;
            }

            transform.position = bldgPos;
        }

    }

    void Respawn(float playerPosition)
    {
        /*
        int randomSign = Random.Range(0, 2);
        if (randomSign == 0)
        {
            randomSign = 1;
        }
        else
        {
            randomSign = -1;
        }
        */
        //float randomX = Random.Range(15f, 25f) * ((randomSign - 0.5f) * 2);

        transform.localScale = (new Vector3(30, 30, 30)) * startingScale;
        transform.position = new Vector3(defaultXpos, -5, 660 + playerPosition);

        actuallyMoving = false;

        //return randomSign;
    }
}

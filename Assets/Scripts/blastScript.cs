using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blastScript : MonoBehaviour
{
    public Rigidbody rb;
    public Rigidbody playerRb;
    public float blastRecharge;
    public bool blastOn;
    private int layermask;
    public float maxRange;
    //public float maxSpread;
    public Vector3 maxScale;
    public Vector3 minScale;
    public float startPosZ;

    public obstacle_movement obstacleMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        // turn off mesh renderer to make invisible when not in use
        GetComponent<MeshRenderer>().enabled = false;

        rb.position = new Vector3(0, -40, -15);
        minScale = new Vector3(1.5f, 1, 1);
        maxScale = new Vector3(3, 1, 1);
        rb.transform.localScale = minScale;
        blastRecharge = 0;
        int obstacles = 1 << LayerMask.NameToLayer("obstacles");
        layermask = obstacles;
        maxRange = 20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (blastOn == true)
        {

            float currentRange = rb.transform.position.z - startPosZ;
            if (currentRange > maxRange)
            {

                rb.position = new Vector3(0, -40, 15);
                rb.transform.localScale = minScale;
                rb.velocity = Vector3.zero;
                GetComponent<MeshRenderer>().enabled = false;
                blastOn = false;
            }
            else
            {
                float interpolate = currentRange / maxRange;
                Vector3 interpolatedScale = Vector3.Lerp(minScale, maxScale, interpolate);
                rb.transform.localScale = interpolatedScale;
            }
        }
        blastRecharge -= Time.deltaTime;

        if (Input.GetKey("s"))
        {
            if (blastOn == false)
            {
                if (blastRecharge < 0)
                {
                    GetComponent<MeshRenderer>().enabled = true;
                    Vector3 playerPos = playerRb.transform.position;
                    startPosZ = playerPos.z;
                    Vector3 playerVel = playerRb.velocity;
                    Blast(playerPos, playerVel);
                    blastRecharge = 2;
                    blastOn = true;
                }
            }

        }
    }

    public void Blast(Vector3 startPos, Vector3 vel)
    {
        rb.transform.localScale = new Vector3(1.5f, 1, 1);
        startPos.y = 1;
        rb.transform.position = startPos;
        vel.y = 0;
        vel.z += 20f;
        vel.x *= 0.5f;

        if (vel.z < 5)
        {
            vel.z = 5;
        }
        rb.velocity = vel;
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody otherRb = other.GetComponent<Rigidbody>();
        Vector3 otherPos = other.transform.position;
        float zdist = otherPos.z - startPosZ;
        float xdist = otherPos.x - rb.transform.position.x;
        Debug.Log("We hit " + other.name + " at: " + xdist + " (x), " + zdist + " (z).");

        Vector3 blastForce;
        //blastForce.z = 10000 / zdist;
        //blastForce.x = 100 * xdist;
        //blastForce.y = (2500 / zdist) - Mathf.Abs(10 * xdist);
        blastForce.z = 1800 + (zdist*-40);
        blastForce.x = 150 * xdist;
        blastForce.y = 500 + (zdist*-15);

        //Vector3 blastTorque;

        //blastTorque.z = 100;
        //blastTorque.x = 100;
        //blastTorque.y = 100;

        otherRb.AddForce(blastForce);
        //otherRb.AddTorque(blastTorque);

        obstacleMovementScript = other.GetComponent<obstacle_movement>();
        obstacleMovementScript.Blasted();
    }
}

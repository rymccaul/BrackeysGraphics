using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle_movement : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject obstacle;
    private GameObject player_obj = null;
    private float pos;
    public float height;
    private float random_lane;
    private int random_lane_int;
    private float random_distance;
    private float player_pos;
    public Collider col;
    private Vector3 spawnCheck_origin;
    private float carSpeed;

    public int destinationLane;
    public int currentLane;

    public bool rightLaneChangeSafe;
    public bool leftLaneChangeSafe;

    public float sizeOfLaneCheckBox;
    private float colliderZdimensionFactor;
    public bool hasCollidedWithPlayer;
    //private float collsionDebugDistLeft;
    //private float collisionDebugDistFront;
    //private float collsionDebugDistRight;
    private bool needToHardResetLane;
    /*
    public float velocityDifferenceRightForward;
    public float timeToCollisionRightForward;
    public float velocityDifferenceLeftForward;
    public float timeToCollisionLeftForward;

    public float velocityDifferenceRightBack;
    public float timeToCollisionRightBack;
    public float velocityDifferenceLeftBack;
    public float timeToCollisionLeftBack;


    public int rightLaneSafetyCount;
    */
    private int layermask;
    private int obstacleLayermask;

    public sensor_script closeSensorScript;

    public Material turnSignal;
    public Material defaultMat;

    public bool needToCheckCurrentLanePreference;
    public bool preferCurrentLaneToRight;
    public bool preferCurrentLaneToLeft;

    public bool gotBlasted;


    public material_controller material_chooser;


    // public GameObject[] numberOfObstacles;

    // Start is called before the first frame update
    void Start()
    {
        // numberOfObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        // random_lane_int = Random.Range(-3, 3);
        // random_lane = (float)random_lane_int;
        // random_distance = Random.Range(100, 150);
        // rb.position = new Vector3(2*random_lane, 1, random_distance);
        rb.velocity = new Vector3(0, 0, -50);
        rightLaneChangeSafe = false;
        leftLaneChangeSafe = false;

        int sensors = 1 << LayerMask.NameToLayer("sensors");
        int players = 1 << LayerMask.NameToLayer("players");
        int obstacles = 1 << LayerMask.NameToLayer("obstacles");
        layermask = sensors | players;
        obstacleLayermask = obstacles;
        layermask = ~layermask;

        if (player_obj == null)
             player_obj = GameObject.FindGameObjectWithTag("Player");

        col = gameObject.GetComponent<Collider>();

        closeSensorScript = gameObject.GetComponentInChildren<sensor_script>();

        //GetComponent<Renderer>().material = defaultMat;
        //material_chooser.brakeOn = false;
        //material_chooser.leftOrRight = 0;

        closeSensorScript.sensorResizeX = 2f;
        closeSensorScript.sensorPosX = 0;

        gotBlasted = false;

        rb.angularDrag = 1.5f;

        sizeOfLaneCheckBox = 20f;
        //layermask = 1 << 12;
        //layermask = ~layermask;

        colliderZdimensionFactor = GetComponent<BoxCollider>().size.z;
        colliderZdimensionFactor /= 2.5f;
        hasCollidedWithPlayer = false;
        needToHardResetLane = false;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        player_pos = player_obj.transform.position.z;
        pos = transform.position.z;
        height = transform.position.y;

        if (pos < player_pos - 8 || height < -30)
        {
            FindNewSpawnPoint();
        }

        if (destinationLane > currentLane)
        {
            float xpos = transform.position.x;
            if (Mathf.Abs((destinationLane * 2) - xpos) < 0.1)
            {

                transform.position = new Vector3((destinationLane * 2), transform.position.y, transform.position.z);
                //rb.AddForce(-75, 0, 0);
                currentLane = destinationLane;

                closeSensorScript.sensorResizeX = 2f;
                closeSensorScript.sensorPosX = 0;

                //GetComponent<Renderer>().material = defaultMat;
                material_chooser.leftOrRight = 0;

                if (needToHardResetLane == false)
                {
                    rb.AddForce(-75, 0, 0);
                }
                else
                {
                    Vector3 preResetVelocity = rb.velocity;
                    HardResetLane(preResetVelocity);
                }
            }
        }
        if (destinationLane < currentLane)
        {
            float xpos = transform.position.x;
            if (Mathf.Abs((destinationLane * 2) - xpos) < 0.1)
            {
                transform.position = new Vector3((destinationLane * 2), transform.position.y, transform.position.z);
                //rb.AddForce(75, 0, 0);
                currentLane = destinationLane;

                closeSensorScript.sensorResizeX = 2f;
                closeSensorScript.sensorPosX = 0;

                //GetComponent<Renderer>().material = defaultMat;
                material_chooser.leftOrRight = 0;
                
                if (needToHardResetLane == false)
                {
                    rb.AddForce(75, 0, 0);
                }
                else
                {
                    Vector3 preResetVelocity = rb.velocity;
                    HardResetLane(preResetVelocity);
                }
            }
        }
    }

    public void changeLanes(float frontTimeToCollision, Collider otherFront, float otherFrontVelocity)
    {
        if (currentLane == destinationLane)             //  if we're currently not in the act of changing lanes ...
        {
            needToCheckCurrentLanePreference = false;
            preferCurrentLaneToLeft = false;
            preferCurrentLaneToRight = false;


            rightLaneChangeSafe = false;
            leftLaneChangeSafe = false;

            Vector3 overlapBoxSize = new Vector3(4f, 0.25f, sizeOfLaneCheckBox);
            Vector3 centerPosition = rb.position;
            centerPosition.z += sizeOfLaneCheckBox - 5f;

            Collider[] proximityCheck = Physics.OverlapBox(centerPosition, overlapBoxSize, Quaternion.identity, layermask);

            List<Collider> proximityCheckList = new List<Collider>(proximityCheck);

            proximityCheckList.Remove(col);
            //proximityCheckList.Remove(otherFront);

            obstacle_movement closeObstacleScript;

            float shortestDistanceLeft = 50f;
            float shortestDistanceRight = 50f;
            float shortestDistanceCurrent = 50f;


            //string myMessage = rb.name + " wants to change lanes, in da house is: ";



            foreach (Collider go in proximityCheckList)
            {
                //if (go != col)
                //{

                if (go.gameObject.layer == 10)
                {
                    //Debug.Log("error, we got em bois");
                    closeObstacleScript = go.gameObject.GetComponent<obstacle_movement>();
                    int closeDestinationLane = closeObstacleScript.destinationLane;
                    int closeLaneDifferential = closeDestinationLane - currentLane;     //closeLaneDifferential = -1 for to the left, 0 for current lane, and +1 for to the right
                    Rigidbody closeRb = go.attachedRigidbody;
                    float closeDistance = closeRb.position.z - transform.position.z;

                    if (closeDistance < 0)
                    {
                        //Vector3 debugRay = go.transform.position - rb.position;
                        //Debug.Log(rb.name + " sees " + go.name + " at a distance of " + closeDistance);
                        //Debug.DrawRay(rb.position, debugRay, Color.green);
                        //Debug.Break();
                        closeDistance = 0;
                    }

                    switch (closeLaneDifferential)
                    {
                        case -1:

                            if (closeDistance < shortestDistanceLeft)
                            {
                                shortestDistanceLeft = closeDistance;
                            }
                            break;

                        case 0:

                            if (closeDistance < shortestDistanceCurrent)
                            {
                                shortestDistanceCurrent = closeDistance;
                            }
                            break;
                        case 1:

                            if (closeDistance < shortestDistanceRight)
                            {
                                shortestDistanceRight = closeDistance;
                            }

                            break;
                        default:
                            //Debug.Log("Error with closeLaneDifferential");
                            break;
                    }
                }
                


                // below this is old
                  

            }

            //collisionDebugDistFront = shortestDistanceCurrent;
            //collsionDebugDistLeft = shortestDistanceLeft;
            //collsionDebugDistRight = shortestDistanceRight;


            if (currentLane != 3)
            {
                if (shortestDistanceRight > 10f)
                {
                    if (shortestDistanceRight > (10f * colliderZdimensionFactor))
                    {
                        rightLaneChangeSafe = true;
                    }
                    else
                    {
                        //Debug.Log("We made a difference, factor is " + colliderZdimensionFactor);
                    }
                }
            }

            if (currentLane != -3)
            {
                if (shortestDistanceLeft > 10f)
                {
                    if (shortestDistanceLeft > (10f * colliderZdimensionFactor))
                    {
                        leftLaneChangeSafe = true;
                    }
                    else
                    {
                        //Debug.Log("We made a difference, factor is " + colliderZdimensionFactor);
                    }
                }
            }

            if (leftLaneChangeSafe == false && rightLaneChangeSafe == false)
            {
                //Debug.Log(rb.name + " has no safe lane to change to!");
                //Debug.Break();
            }

            if(rightLaneChangeSafe == true)
            {
                if(shortestDistanceRight < shortestDistanceCurrent)
                {
                    rightLaneChangeSafe = false;
                }
            }
            if(leftLaneChangeSafe == true)
            {
                if(shortestDistanceLeft < shortestDistanceCurrent)
                {
                    leftLaneChangeSafe = false;
                }
            }
            if(leftLaneChangeSafe == true && rightLaneChangeSafe == true)
            {
                if(shortestDistanceLeft < shortestDistanceRight)
                {
                    leftLaneChangeSafe = false;
                }
                else
                {
                    rightLaneChangeSafe = false;
                }
            }

            if(rightLaneChangeSafe == true)
            {
                destinationLane++;
                rb.AddForce(75, 0, 0);

                closeSensorScript.sensorResizeX = 1.5f;
                closeSensorScript.sensorPosX = 0.25f;

                //GetComponent<Renderer>().material = turnSignal;
                material_chooser.leftOrRight = 1;
            }else if (leftLaneChangeSafe == true)
            {
                destinationLane--;
                rb.AddForce(-75, 0, 0);

                closeSensorScript.sensorResizeX = 1.5f;
                closeSensorScript.sensorPosX = -0.25f;

                //GetComponent<Renderer>().material = turnSignal;
                material_chooser.leftOrRight = -1;
            }
            else
            {
                //Debug.Log(rb.name + " didn't want to change lanes.");
            }

            // draw rays to see size of laneCheck box
            //Debug.DrawRay(centerPosition, Vector3.forward * sizeOfLaneCheckBox, Color.magenta);
            //Debug.DrawRay(centerPosition, Vector3.back * sizeOfLaneCheckBox, Color.magenta);
            //Debug.DrawRay(rb.position, Vector3.up * 10f, Color.magenta);
            //Debug.Break();
        }
    }

    public void Blasted()
    {
        //Debug.Log("Blasted works!");
        currentLane = destinationLane;
        rightLaneChangeSafe = false;
        leftLaneChangeSafe = false;
        gotBlasted = true;
        hasCollidedWithPlayer = true;
    }

    private void HardResetLane(Vector3 preResetVel)
    {
        preResetVel.x = 0;
        rb.velocity = preResetVel;
        rb.angularVelocity = Vector3.zero;
        rb.transform.rotation = Quaternion.identity;
    }

    private void FindNewSpawnPoint()
    {
        random_lane_int = Random.Range(-3, 4);
        random_lane = (float)random_lane_int;
        random_distance = Random.Range(player_pos + 100, player_pos + 150);

        spawnCheck_origin = new Vector3(2 * random_lane, 1, random_distance); //+ 4f);

        Vector3 spawnCheckBoxSize = new Vector3(1f, 0.25f, 4f);
        Vector3 spawnCheckCenterPosition = spawnCheck_origin;

        Collider[] spawnCheck = Physics.OverlapBox(spawnCheckCenterPosition, spawnCheckBoxSize, Quaternion.identity, layermask);

        /*
        Vector3 spawnCheckVisualizerCenter = spawnCheckCenterPosition;
        spawnCheckVisualizerCenter.z += 4f;

        Vector3 spawnCheckVisualizerRight = spawnCheckVisualizerCenter;
        spawnCheckVisualizerRight.x += 1f;

        Vector3 spawnCheckVisualizerLeft = spawnCheckVisualizerCenter;
        spawnCheckVisualizerLeft.x -= 1f;

        Debug.DrawRay(spawnCheckVisualizerCenter, Vector3.back * 8f);
        Debug.DrawRay(spawnCheckVisualizerLeft, Vector3.back * 8f);
        Debug.DrawRay(spawnCheckVisualizerRight, Vector3.back * 8f);
        */
        if (spawnCheck.Length != 0)
        {
            //Debug.Log("Spawn bad.");
        }
        else
        {
            rb.position = new Vector3(2 * random_lane, 1, random_distance);

            transform.rotation = Quaternion.identity;
            rb.angularVelocity = Vector3.zero;

            //rb.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            carSpeed = Random.Range(-10f, -40f);
            rb.velocity = new Vector3(0, 0, carSpeed);

            currentLane = random_lane_int;
            destinationLane = random_lane_int;

            rightLaneChangeSafe = false;
            leftLaneChangeSafe = false;

            //GetComponent<Renderer>().material = defaultMat;
            material_chooser.brakeOn = false;
            material_chooser.leftOrRight = 0;

            closeSensorScript.sensorResizeX = 2f;
            closeSensorScript.sensorPosX = 0;

            closeSensorScript.obstacleRespawn();

            gotBlasted = false;
            hasCollidedWithPlayer = false;
            needToHardResetLane = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        if (gotBlasted == true)
        {
            if (collisionGameObject.layer == 8)
            {
                float blastPos = rb.transform.position.x;
                float xVel = rb.velocity.x;

                if (xVel >= 0)
                {
                    if (blastPos < -6)
                    {
                        destinationLane = -3;
                        currentLane = -4;
                    }else if (blastPos < -4)
                    {
                        destinationLane = -2;
                        currentLane = -3;
                    }else if (blastPos < -2)
                    {
                        destinationLane = -1;
                        currentLane = -2;
                    }else if (blastPos < 0)
                    {
                        destinationLane = 0;
                        currentLane = -1;
                    }else if (blastPos < 2)
                    {
                        destinationLane = 1;
                        currentLane = 0;
                    }else if (blastPos < 4)
                    {
                        destinationLane = 2;
                        currentLane = 1;
                    }else if (blastPos < 6)
                    {
                        destinationLane = 3;
                        currentLane = 2;
                    }
                    else
                    {
                        destinationLane = 3;
                        currentLane = 4;
                    }
                }
                else
                {
                    if (blastPos < -6)
                    {
                        destinationLane = -3;
                        destinationLane = -4;
                    }else if (blastPos < -4)
                    {
                        destinationLane = -3;
                        currentLane = -2;
                    }else if (blastPos < -2)
                    {
                        destinationLane = -2;
                        currentLane = -1;
                    }else if (blastPos < 0)
                    {
                        destinationLane = -1;
                        currentLane = 0;
                    }else if (blastPos < 2)
                    {
                        destinationLane = 0;
                        currentLane = 1;
                    }else if (blastPos < 4)
                    {
                        destinationLane = 1;
                        currentLane = 2;
                    }else if (blastPos < 6)
                    {
                        destinationLane = 2;
                        currentLane = 3;
                    }
                    else
                    {
                        destinationLane = 3;
                        currentLane = 4;
                    }
                }
                gotBlasted = false;
                //needToHardResetLane = true;
            }

            
        }

        if (collisionGameObject == player_obj)
        {
            hasCollidedWithPlayer = true;
        }else if(collisionGameObject.layer == 10)
        {
            if (collisionGameObject.GetComponent<obstacle_movement>().hasCollidedWithPlayer)
            {
                hasCollidedWithPlayer = true;
            }
        }
        Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;
        float collisionForceMagnitude = collisionForce.magnitude;

        //Debug.Log(rb.name + " collided with " + collision.collider.gameObject.name + ", with force of " + collisionForceMagnitude);

        if (hasCollidedWithPlayer == false)
        {
            if (collisionGameObject.layer == 10)
            {
                //string collisionDebug;
                //Debug.DrawRay(rb.position, Vector3.up * 10f, Color.magenta);
                //collisionDebug = rb.name + " dest: " + destinationLane + ", cur: " + currentLane;
                //Debug.Log(rb.name + " dest: " + destinationLane + ", cur: " + currentLane);

                if (currentLane != destinationLane)
                {
                    //collisionDebug += (", left/current/right distances = " + collsionDebugDistLeft + "/" + collisionDebugDistFront + "/" + collsionDebugDistRight);
                    int currentLaneHolder = currentLane;
                    int destinationLaneHolder = destinationLane;

                    currentLane = destinationLaneHolder;
                    destinationLane = currentLaneHolder;

                    Vector3 newVelocity = rb.velocity;
                    newVelocity.x = 0;
                    rb.velocity = newVelocity;
                    if (currentLane > destinationLane)
                    {
                        rb.AddForce(-75f, 0, 0);
                    }
                    else
                    {
                        rb.AddForce(75f, 0, 0);
                    }
                    needToHardResetLane = true;
                }
                else
                {
                    Vector3 preResetVelocity = rb.velocity;
                    HardResetLane(rb.velocity);
                }

                //Debug.Log(collisionDebug);
                //Debug.Break();

            }

        }

    }


}

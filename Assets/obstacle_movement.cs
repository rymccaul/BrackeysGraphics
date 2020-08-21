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

    public sensor_script closeSensorScript;

    public Material turnSignal;
    public Material defaultMat;

    public bool needToCheckCurrentLanePreference;
    public bool preferCurrentLaneToRight;
    public bool preferCurrentLaneToLeft;

    public bool gotBlasted;


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
        layermask = sensors | players;
        layermask = ~layermask;

        if (player_obj == null)
             player_obj = GameObject.FindGameObjectWithTag("Player");

        col = gameObject.GetComponent<Collider>();

        closeSensorScript = gameObject.GetComponentInChildren<sensor_script>();

        GetComponent<Renderer>().material = defaultMat;

        closeSensorScript.sensorResizeX = 2f;
        closeSensorScript.sensorPosX = 0;

        gotBlasted = false;

        rb.angularDrag = 1.5f;

        //layermask = 1 << 12;
        //layermask = ~layermask;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        player_pos = player_obj.transform.position.z;
        pos = transform.position.z;
        height = transform.position.y;
        if (pos < player_pos - 8 || height < -30)
        {
            //float rayLength = 8f;
            random_lane_int = Random.Range(-3, 4);
            random_lane = (float)random_lane_int;
            random_distance = Random.Range(100, 150);

            spawnCheck_origin = new Vector3(2 * random_lane, 1, random_distance); //+ 4f);

            Vector3 spawnCheckBoxSize = new Vector3(1f, 0.25f, 4f);
            Vector3 spawnCheckCenterPosition = spawnCheck_origin;

            Collider[] spawnCheck = Physics.OverlapBox(spawnCheckCenterPosition, spawnCheckBoxSize, Quaternion.identity, layermask);

            Vector3 spawnCheckVisualizerCenter = spawnCheckCenterPosition;
            spawnCheckVisualizerCenter.z += 4f;

            Vector3 spawnCheckVisualizerRight = spawnCheckVisualizerCenter;
            spawnCheckVisualizerRight.x += 1f;

            Vector3 spawnCheckVisualizerLeft = spawnCheckVisualizerCenter;
            spawnCheckVisualizerLeft.x -= 1f;

            Debug.DrawRay(spawnCheckVisualizerCenter, Vector3.back * 8f);
            Debug.DrawRay(spawnCheckVisualizerLeft, Vector3.back * 8f);
            Debug.DrawRay(spawnCheckVisualizerRight, Vector3.back * 8f);

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

                GetComponent<Renderer>().material = defaultMat;

                closeSensorScript.sensorResizeX = 2f;
                closeSensorScript.sensorPosX = 0;

                closeSensorScript.obstacleRespawn();

                gotBlasted = false;
            }

            /*
            if (Physics.Raycast(spawnCheck_origin, Vector3.back, rayLength) == true)
            {
                //Debug.Log("Spawn bad");
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

                GetComponent<Renderer>().material = defaultMat;

                closeSensorScript.sensorResizeX = 2f;
                closeSensorScript.sensorPosX = 0;

                closeSensorScript.obstacleRespawn();

            }
            */

        }

        if (destinationLane > currentLane)
        {
            float xpos = transform.position.x;
            if (Mathf.Abs((destinationLane * 2) - xpos) < 0.1)
            {
                transform.position = new Vector3((destinationLane * 2), transform.position.y, transform.position.z);
                rb.AddForce(-75, 0, 0);
                currentLane = destinationLane;

                closeSensorScript.sensorResizeX = 2f;
                closeSensorScript.sensorPosX = 0;

                GetComponent<Renderer>().material = defaultMat;
            }
        }
        if (destinationLane < currentLane)
        {
            float xpos = transform.position.x;
            if (Mathf.Abs((destinationLane * 2) - xpos) < 0.1)
            {
                transform.position = new Vector3((destinationLane * 2), transform.position.y, transform.position.z);
                rb.AddForce(75, 0, 0);
                currentLane = destinationLane;

                closeSensorScript.sensorResizeX = 2f;
                closeSensorScript.sensorPosX = 0;

                GetComponent<Renderer>().material = defaultMat;
            }
        }
    }

    public void changeLanes(float frontTimeToCollision, Collider otherFront, float otherFrontVelocity)
    {
        if (currentLane == destinationLane)
        {
            needToCheckCurrentLanePreference = false;
            preferCurrentLaneToLeft = false;
            preferCurrentLaneToRight = false;


            rightLaneChangeSafe = false;
            leftLaneChangeSafe = false;

            Vector3 overlapBoxSize = new Vector3(4f, 0.25f, 10f);
            Vector3 centerPosition = rb.position;

            Collider[] proximityCheck = Physics.OverlapBox(centerPosition, overlapBoxSize, Quaternion.identity, layermask);

            List<Collider> proximityCheckList = new List<Collider>(proximityCheck);

            proximityCheckList.Remove(col);
            proximityCheckList.Remove(otherFront);

            obstacle_movement closeObstacleScript = otherFront.gameObject.GetComponent<obstacle_movement>();
            int frontDestinationLane = closeObstacleScript.destinationLane;
            int frontCurrentlane = closeObstacleScript.currentLane;

            float leftTimeToCollision = 50f;
            float rightTimeToCollision = 50f;

            if (frontDestinationLane == frontCurrentlane) // the obstacle in front is not changing lanes
            {
                needToCheckCurrentLanePreference = true;
            }
            else if (frontDestinationLane > frontCurrentlane) // the obstacle in front is changing lanes to the right
            {
                rightTimeToCollision = frontTimeToCollision;
            }
            else if (frontDestinationLane < frontCurrentlane) // the obstalce in front is changing lanes to the left
            {
                leftTimeToCollision = frontTimeToCollision;
            }


            //string myMessage = rb.name + " wants to change lanes, in da house is: ";



            foreach (Collider go in proximityCheckList)
            {
                //if (go != col)
                //{

                    closeObstacleScript = go.gameObject.GetComponent<obstacle_movement>();
                    int closeDestinationLane = closeObstacleScript.destinationLane;
                    int closeCurrentLane = closeObstacleScript.currentLane;

                    //myMessage += go.name + " (destlane = " + closeDestinationLane;

                    if (closeDestinationLane == (currentLane + 1))
                    {
                        Rigidbody closeRb = go.attachedRigidbody;
                        float distanceFromCenter = closeRb.position.z - transform.position.z;

                        if (distanceFromCenter < 2 && distanceFromCenter > -2)
                        {
                            //myMessage += ", right collision hazard is guaranteed";
                            rightTimeToCollision = 0;
                            //Debug.Break();
                        }
                        else
                        {

                            float closeVelocityDifferenceRight = rb.velocity.z - closeRb.velocity.z;

                            if (closeVelocityDifferenceRight != 0)
                            {
                                float newRightTimeToCollision = distanceFromCenter / closeVelocityDifferenceRight;
                                //myMessage += ", right time to collision = " + newRightTimeToCollision;
                                if (newRightTimeToCollision > 0 && newRightTimeToCollision < rightTimeToCollision)
                                {
                                    rightTimeToCollision = newRightTimeToCollision;
                                    if (needToCheckCurrentLanePreference == true)
                                    {
                                        preferCurrentLaneToRight = false;

                                        if (distanceFromCenter > 0)
                                        {
                                            if (closeRb.velocity.z < otherFrontVelocity)
                                            {
                                                preferCurrentLaneToRight = true;
                                            }
                                        }

                                    }
                                }
                            }

                        }
                        //else if (distanceFromCenter < 1 && distanceFromCenter > -1)
                        //{
                        //    rightTimeToCollision = 0;
                        //    myMessage += ", right collision hazard is guaranteed";
                        //}


                    }
                    else if (closeDestinationLane == (currentLane - 1))
                    {
                        Rigidbody closeRb = go.attachedRigidbody;
                        float distanceFromCenter = closeRb.position.z - transform.position.z;

                        if (distanceFromCenter < 2 && distanceFromCenter > -2)
                        {
                            //myMessage += ", left collision hazard is guaranteed";
                            leftTimeToCollision = 0;
                            //Debug.Break();
                        }
                        else
                        {

                            float closeVelocityDifferenceLeft = rb.velocity.z - closeRb.velocity.z;

                            if (closeVelocityDifferenceLeft != 0)
                            {
                                float newLeftTimeToCollision = distanceFromCenter / closeVelocityDifferenceLeft;
                                //myMessage += ", left time to collision = " + newLeftTimeToCollision;
                                if (newLeftTimeToCollision > 0 && newLeftTimeToCollision < leftTimeToCollision)
                                {
                                    leftTimeToCollision = newLeftTimeToCollision;
                                    if (needToCheckCurrentLanePreference == true)
                                    {
                                        preferCurrentLaneToLeft = false;

                                        if (distanceFromCenter > 0)
                                        {
                                            if (closeRb.velocity.z < otherFrontVelocity)
                                            {
                                                preferCurrentLaneToLeft = true;
                                            }
                                        }

                                    }
                                }
                            }

                        }
                    }

                    else if (closeDestinationLane == currentLane && closeDestinationLane != closeCurrentLane)
                    {
                        if (closeDestinationLane > closeCurrentLane) // then they must be changing lanes to the right
                        {
                            Rigidbody closeRb = go.attachedRigidbody;
                            float distanceFromCenter = closeRb.position.z - transform.position.z;

                            if (distanceFromCenter < 1 && distanceFromCenter > -1)
                            {
                                leftTimeToCollision = 0;
                            }
                            else
                            {
                                float closeVelocityDifferenceLeft = rb.velocity.z - closeRb.velocity.z;

                                if (closeVelocityDifferenceLeft != 0)
                                {
                                    float newLeftTimeToCollision = distanceFromCenter / closeVelocityDifferenceLeft;
                                    if (newLeftTimeToCollision > 0 && newLeftTimeToCollision < leftTimeToCollision)
                                    {
                                        leftTimeToCollision = newLeftTimeToCollision;

                                    }
                                }
                            }
                        }
                        else if (closeDestinationLane < closeCurrentLane) // then they must be changing lanes to the left
                        {
                            Rigidbody closeRb = go.attachedRigidbody;
                            float distanceFromCenter = closeRb.position.z - transform.position.z;

                            if (distanceFromCenter < 1 && distanceFromCenter > -1)
                            {
                                rightTimeToCollision = 0;
                            }
                            else
                            {
                                float closeVelocityDifferenceRight = rb.velocity.z - closeRb.velocity.z;
                                if (closeVelocityDifferenceRight != 0)
                                {
                                    float newRightTimeToCollision = distanceFromCenter / closeVelocityDifferenceRight;
                                    if (newRightTimeToCollision > 0 && newRightTimeToCollision < rightTimeToCollision)
                                    {
                                        rightTimeToCollision = newRightTimeToCollision;
                                    }
                                }
                            }
                        }
                    }


                    //myMessage += "); ";

                //}
                //Debug.Log(rb.name + " wants to change lanes, " + go.name + " in da house");
            }

            //Debug.Log(myMessage);
            //Debug.Break();


            if (currentLane != 3)
            {
                if (rightTimeToCollision < 0)
                {
                    rightLaneChangeSafe = true;
                }
                else if (rightTimeToCollision > 2f)
                {
                    if (preferCurrentLaneToRight == false)
                    {

                        rightLaneChangeSafe = true;
                    }
                    else
                    {
                        //Debug.Log(rb.name + " likes current lane more than right.");
                    }
                    /*
                    if (rightTimeToCollision > frontTimeToCollision)
                    {
                        rightLaneChangeSafe = true;
                    }
                    else
                    {
                        Debug.Log(rb.name + " likes current lane more than right.");
                        //Debug.Break();
                    }
                    */
                }
            }

            if (currentLane != -3)
            {
                if (leftTimeToCollision < 0 )
                {
                    leftLaneChangeSafe = true;
                }
                else if (leftTimeToCollision > 2f)
                {
                    if (preferCurrentLaneToLeft == false)
                    {

                        leftLaneChangeSafe = true;
                    }
                    else
                    {
                        //Debug.Log(rb.name + " likes current lane more than left.");
                    }
                    /*
                    if (leftTimeToCollision > frontTimeToCollision)
                    {
                        leftLaneChangeSafe = true;
                    }
                    else
                    {
                        Debug.Log(rb.name + " likes current lane more than left.");
                        //Debug.Break();
                    }
                    */
                }



                /*
                if (leftTimeToCollision < 0 || leftTimeToCollision > 2f)
                {
                    leftLaneChangeSafe = true;
                }
                */
            }

            if (leftLaneChangeSafe == false && rightLaneChangeSafe == false)
            {
                //Debug.Log(rb.name + " has no safe lane to change to!");
                //Debug.Break();
            }

            if (rightLaneChangeSafe == true)
            {
                if (leftLaneChangeSafe == false)
                {
                    destinationLane++;
                    rb.AddForce(75, 0, 0);

                    closeSensorScript.sensorResizeX = 1.5f;
                    closeSensorScript.sensorPosX = 0.25f;

                    GetComponent<Renderer>().material = turnSignal;

                }
                else
                {
                    if (rightTimeToCollision > leftTimeToCollision)
                    {
                        destinationLane++;
                        rb.AddForce(75, 0, 0);

                        closeSensorScript.sensorResizeX = 1.5f;
                        closeSensorScript.sensorPosX = 0.25f;

                        GetComponent<Renderer>().material = turnSignal;
                    }
                    else if (rightTimeToCollision != leftTimeToCollision)
                    {
                        destinationLane--;
                        rb.AddForce(-75, 0, 0);

                        closeSensorScript.sensorResizeX = 1.5f;
                        closeSensorScript.sensorPosX = -0.25f;

                        GetComponent<Renderer>().material = turnSignal;
                    }
                    else
                    {
                        int leftOrRight = Random.Range(1, 3);
                        if (leftOrRight == 1)
                        {
                            destinationLane--;
                            rb.AddForce(-75, 0, 0);

                            closeSensorScript.sensorResizeX = 1.5f;
                            closeSensorScript.sensorPosX = -0.25f;

                            GetComponent<Renderer>().material = turnSignal;
                        }
                        else
                        {
                            destinationLane++;
                            rb.AddForce(75, 0, 0);

                            closeSensorScript.sensorResizeX = 1.5f;
                            closeSensorScript.sensorPosX = 0.25f;

                            GetComponent<Renderer>().material = turnSignal;
                        }
                    }

                    /*
                    int leftOrRight = Random.Range(0, 2);
                    if (leftOrRight == 0)
                    {
                        Debug.Log(rb.name + "went to to a tossup and chose left.");
                        destinationLane--;
                        rb.AddForce(-75, 0, 0);
                    }
                    else
                    {
                        Debug.Log(rb.name + "went to to a tossup and chose right.");
                        destinationLane++;
                        rb.AddForce(75, 0, 0);

                    }
                    */
                }
                //Debug.Break();
            }
            else if (leftLaneChangeSafe == true)
            {
                destinationLane--;
                rb.AddForce(-75, 0, 0);

                GetComponent<Renderer>().material = turnSignal;
                //Debug.Break();
            }

        }
    }

    public void Blasted()
    {
        Debug.Log("Blasted works!");
        currentLane = destinationLane;
        rightLaneChangeSafe = false;
        leftLaneChangeSafe = false;
        gotBlasted = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (gotBlasted == true)
        {
            if (collision.gameObject.layer == 8)
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
                /*
                if (blastPos < -5)
                {
                    destinationLane = -3;
                }else if (blastPos < -3)
                {
                    destinationLane = -2;
                }else if (blastPos < -1)
                {
                    destinationLane = -1;
                }else if (blastPos < 1)
                {
                    destinationLane = 0;
                }else if (blastPos < 3)
                {
                    destinationLane = 1;
                }else if (blastPos < 5)
                {
                    destinationLane = 2;
                }else if (blastPos > 5)
                {
                    destinationLane = 3;
                }


                if (blastPos > (2 * destinationLane))
                {
                    currentLane = destinationLane + 1;
                }
                else
                {
                    currentLane = destinationLane - 1;
                }
                */
                gotBlasted = false;
            }
        }

    }


}



    /*
    public void changeLanes()
    {
        //Debug.Log("It's time to for " + rb.name + " to change lanes.");

        rightLaneChangeSafe = false;
        leftLaneChangeSafe = false;

        rightLaneSafetyCount = 0;
        int leftLaneSafetyCount = 0;

        float lanecheckRayLength = 10f;

        if (destinationLane < 3)
        {
            Vector3 RightLaneChecker = new Vector3(transform.position.x + 2f, 1, transform.position.z);

            Ray rightForward = new Ray(RightLaneChecker, Vector3.forward);
            Ray rightBack = new Ray(RightLaneChecker, Vector3.back);

            Debug.DrawRay(RightLaneChecker, Vector3.forward * lanecheckRayLength);
            Debug.DrawRay(RightLaneChecker, Vector3.back * lanecheckRayLength);

            RaycastHit rightForwardHit;
            RaycastHit rightBackwardHit;

            if (Physics.Raycast(rightForward, out rightForwardHit, lanecheckRayLength, layermask) == true)
            {
                if (rightForwardHit.distance > 1)
                {
                    velocityDifferenceRightForward = rb.velocity.z - rightForwardHit.rigidbody.velocity.z;
                    timeToCollisionRightForward = rightForwardHit.distance / velocityDifferenceRightForward;

                    if (timeToCollisionRightForward < 0 || timeToCollisionRightForward > 1)
                    {
                        rightLaneSafetyCount++;
                    }

                }
            }
            else
            {
                rightLaneSafetyCount++;
            }

            if (rightLaneSafetyCount == 1)
            {
                if (Physics.Raycast(rightBack, out rightBackwardHit, lanecheckRayLength, layermask) == true)
                {
                    if (rightBackwardHit.distance > 1)
                    {

                        velocityDifferenceRightBack = rb.velocity.z - rightBackwardHit.rigidbody.velocity.z;
                        timeToCollisionRightBack = rightBackwardHit.distance / velocityDifferenceRightBack;

                        if (timeToCollisionRightForward > 0 || timeToCollisionRightBack < -1)
                        {
                        rightLaneSafetyCount++;
                        }

                    }
                }
                else
                {
                    rightLaneSafetyCount++;
                }
            }

        if (rightLaneSafetyCount == 2)
        {
            rightLaneChangeSafe = true;
            //Debug.Log(rb.name + " is safe to change lanes to the right.");
        }


        }

        if (destinationLane > -3)
        {
            Vector3 LeftLaneChecker = new Vector3(transform.position.x - 2f, 1, transform.position.z);

            Ray leftForward = new Ray(LeftLaneChecker, Vector3.forward);
            Ray leftBack = new Ray(LeftLaneChecker, Vector3.back);

            Debug.DrawRay(LeftLaneChecker, Vector3.forward * lanecheckRayLength);
            Debug.DrawRay(LeftLaneChecker, Vector3.back * lanecheckRayLength);

            RaycastHit leftForwardHit;
            RaycastHit leftBackwardHit;

            if (Physics.Raycast(leftForward, out leftForwardHit, lanecheckRayLength, layermask) == true)
            {
                if (leftForwardHit.distance > 1)
                {
                    velocityDifferenceLeftForward = rb.velocity.z - leftForwardHit.rigidbody.velocity.z;
                    timeToCollisionLeftForward = leftForwardHit.distance / velocityDifferenceLeftForward;

                    if (timeToCollisionLeftForward < 0 || timeToCollisionLeftForward > 1)
                    {
                        leftLaneSafetyCount++;
                    }

                }
            }
            else
            {
                leftLaneSafetyCount++;
            }

            if (leftLaneSafetyCount == 1)
            {
                if (Physics.Raycast(leftBack, out leftBackwardHit, lanecheckRayLength, layermask) == true)
                {
                    if (leftBackwardHit.distance > 1)
                    {

                        velocityDifferenceLeftBack = rb.velocity.z - leftBackwardHit.rigidbody.velocity.z;
                        timeToCollisionLeftBack = leftBackwardHit.distance / velocityDifferenceLeftBack;

                        if (timeToCollisionLeftForward > 0 || timeToCollisionLeftBack < -1)
                        {
                            leftLaneSafetyCount++;
                        }

                    }
                }
                else
                {
                    leftLaneSafetyCount++;
                }
            }

            if (leftLaneSafetyCount == 2)
            {
                leftLaneChangeSafe = true;
                //Debug.Log(rb.name + " is safe to change lanes to the left.");
            }


        }

        if (rightLaneChangeSafe == true)
        {
            if (leftLaneChangeSafe == false)
            {
                destinationLane++;
                rb.AddForce(75, 0, 0);
            }
            else
            {
                int leftOrRight = Random.Range(0, 2);
                if (leftOrRight == 0)
                {
                    //Debug.Log(rb.name + "went to to a tossup and chose left.");
                    destinationLane--;
                    rb.AddForce(-75, 0, 0);
                }
                else
                {
                    //Debug.Log(rb.name + "went to to a tossup and chose right.");
                    destinationLane++;
                    rb.AddForce(75, 0, 0);

                }
            }
            //Debug.Break();
        }
        else if (leftLaneChangeSafe == true)
        {
            destinationLane--;
            rb.AddForce(-75, 0, 0);
            //Debug.Break();
        }

        //  DELETE BELOW??? OR KEEEP?
        if (rightLaneChangeSafe == true && leftLaneChangeSafe == false)
        {
            destinationLane++;
            rb.AddForce(75, 0, 0);
        }
        else if (rightLaneChangeSafe == false && leftLaneChangeSafe == true)
        {
            destinationLane--;
            rb.AddForce(-75, 0, 0);
        }
        else
        // DELETE ABOVE ??? OR KEEP???
    }


        // DELETE BELOW?? OR KEEP>>>
        if (Physics.Raycast(rightForward, out rightForwardHit, lanecheckRayLength) == true || Physics.Raycast(rightBack, out rightBackwardHit, lanecheckRayLength) == true)
        {

            velocityDifferenceRightForward = rightForwardHit.rigidbody.velocity.z
            destinationLane++;
            rb.AddForce(75, 0, 0);
        }
        else
        {

        }
        // DELETE ABOVE??? OR KEEP??
        */

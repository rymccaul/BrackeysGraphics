using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sensor_script : MonoBehaviour
{

    //  the "sensor" is a long trigger that is parented to each individual obstacle.  it senses when the obstacle is on a collision course, and if so, it causes the obstacle to slow down.

    public GameObject otherObstacle;    

    public Rigidbody parRb;
    public float distanceBetween;
    
    public float parVelocity;
    public float otherVelocity;
    public float velocityDifference;
    public float brakeForce;
    public float actualBrakeForce;
    public float timeToCollision;
    public Vector3 colpos;
    //public int numberOfCollisionObjects;
    public Collider otherUpdated;
    public bool tooManyObstacles;

    public obstacle_movement laneChanger;
    //public bool timeToChangeLanes;
    
    public List<Collider> triggerList = new List<Collider>();

    public Collider parCol;

    public int driverAggressionFactor;

    public float accelerationForce;

    public float sensorResizeX;
    public float sensorPosX;

    public float laneChangeRecharge;

    public int layermask;

    //public int waitOneFrame;

    public int expensiveFunctCount;

    public float maxSpeed;


    public material_controller material_chooser;



    // Start is called before the first frame update
    void Start()
    {
        
        parRb = transform.parent.GetComponent<Rigidbody>();
        parCol = transform.parent.GetComponent<Collider>();
        Vector3 startVelocity = parRb.velocity;
        
        //numberOfCollisionObjects = 0;
        tooManyObstacles = false;

        laneChangeRecharge = 1f;

        Physics.IgnoreCollision(GetComponent<Collider>(), parCol);

        driverAggressionFactor = Random.Range(1, 7); // 7 is aggressive driver, 1 is safe driver
        sensorResizeX = 2f;
        sensorPosX = 0;

        int sensors = 1 << LayerMask.NameToLayer("sensors");
        int players = 1 << LayerMask.NameToLayer("players");

        layermask = sensors | players;
        layermask = ~layermask;
        expensiveFunctCount++;

        maxSpeed = Random.Range(-10f, 0f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        laneChangeRecharge -= Time.deltaTime;

        if (triggerList.Count == 0)
        {
            colpos = transform.localPosition;
            colpos.z = 25.5f;
            colpos.x = sensorPosX;
            transform.localPosition = colpos;
            transform.localScale = new Vector3(sensorResizeX, 0.5f, 50f);
            otherUpdated = null;
            material_chooser.brakeOn = false;

            float currentSpeed = parRb.velocity.z;
            
            if (currentSpeed < maxSpeed)
            {
                accelerationForce = HitTheGas(currentSpeed, maxSpeed, timeToCollision, distanceBetween);
                parRb.AddForce(0, 0, accelerationForce * Time.deltaTime);
                
                //accelerationForce = 100f;
                //parRb.AddForce(0, 0, accelerationForce * Time.deltaTime);
            }
            
            // DONE: reset sensor size and set otherUpdated to NULL (this should not go in onTriggerExit)
            // let's make sure the sensor follows the parent (this should be automatic)

        }
        else if (triggerList.Count > 1)  //consider using a while loop here
        {

            tooManyObstaclesFunction();

        }

        if (triggerList.Count == 1)
        {
            if (otherUpdated == null)
            {
                Debug.Log("We've got an issue with nulls");
            }
            if (triggerList[0] != otherUpdated)
            {
                otherUpdated = triggerList[0];
                //Debug.Log(parRb.name + " is focusing on " + otherUpdated.name);
            }
            //check if otherUpdated is NULL, check if otherUpdated is triggerList[1];
            // if our checks make sense, let's start adjusting the sensor size
        }

        if (laneChanger.gotBlasted == false)
        {



            if (otherUpdated != null)//( triggerList.Count != 0 ) //if (otherUpdated != NULL)
            {
                distanceBetween = otherUpdated.transform.position.z - transform.parent.position.z;

                if (distanceBetween >= 0 && distanceBetween < 52f)
                {
                    transform.localScale = new Vector3(sensorResizeX, 0.5f, distanceBetween - 0.9f);
                    parVelocity = parRb.velocity.z;


                    otherVelocity = otherUpdated.GetComponent<Rigidbody>().velocity.z;

                    colpos = transform.localPosition;
                    colpos.z = (((distanceBetween / 2) + 0.1f));//+ 0.5f));
                    colpos.x = sensorPosX;
                    transform.localPosition = colpos;

                    velocityDifference = -otherVelocity + parVelocity;
                    if (velocityDifference != 0)
                    {
                        timeToCollision = distanceBetween / velocityDifference;
                    }
                    else
                    {
                        timeToCollision = 9999f;
                    }

                    if (distanceBetween < 5f)
                    {
                        float tailgateForce = PreventTailgate(distanceBetween);
                        parRb.AddForce(0, 0, -tailgateForce * Time.deltaTime);

                    }

                }
                else
                {
                    if (distanceBetween < 0)
                    {
                        //Debug.Log(parRb.name + " tried to have a negative scale... instead the scale is: " + transform.localScale.z + ", and it thinks it is colliding with " + otherUpdated.name);
                        transform.localScale = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        //Debug.Log(parRb.name + " is paying attention to an obstacle further than tooManyObstacles() can reach.");
                        colpos = transform.localPosition;
                        colpos.z = 25.5f;
                        colpos.x = sensorPosX;
                        transform.localPosition = colpos;
                        transform.localScale = new Vector3(sensorResizeX, 0.5f, 50f);

                    }
                    //Debug.Break();
                    //transform.localScale = new Vector3(0, 0, 0);
                }

                if (timeToCollision > 0 && timeToCollision < 3f)
                {

                    brakeForce = ApplyBrakes(timeToCollision);

                    //Vector3 setVelocity = parRb.velocity;
                    //setVelocity.z = otherVelocity;
                    //parRb.velocity = setVelocity;

                    if (laneChangeRecharge < 0)
                    {
                        laneChanger.changeLanes(0.01f, otherUpdated, otherVelocity);
                        laneChangeRecharge = 1f;
                    }

                    /*
                    brakeForce = 0f;
                    if (distanceBetween < 5f)
                    {
                        Vector3 setVelocity = parRb.velocity;
                        setVelocity.z = otherVelocity;
                        parRb.velocity = setVelocity;

                        if (laneChangeRecharge < 0 )
                        {
                            laneChanger.changeLanes(0.01f, otherUpdated, otherVelocity);
                            laneChangeRecharge = 1f;
                        }
                    }
                    else if (timeToCollision > 4f)
                    {
                        if (parVelocity < maxSpeed)
                        {
                            accelerationForce = 100f;
                            parRb.AddForce(0, 0, accelerationForce * Time.deltaTime);
                        }
                        //accelerate
                    }
                    else if (timeToCollision > 3f)
                    {
                        brakeForce = 100f / timeToCollision;

                        if (laneChangeRecharge < 0 )
                        {
                            laneChanger.changeLanes(timeToCollision, otherUpdated, otherVelocity);
                            laneChangeRecharge = 1f;

                        }
                    }
                    else if (timeToCollision > 2f)
                    {
                        brakeForce = 200f / timeToCollision;
                        if (laneChangeRecharge < 0 )
                        {
                            laneChanger.changeLanes(timeToCollision, otherUpdated, otherVelocity);
                            laneChangeRecharge = 1f;

                        }
                    }
                    else
                    {
                        brakeForce = 400f / timeToCollision;

                        if (laneChangeRecharge < 0 )
                        {

                            laneChanger.changeLanes(timeToCollision, otherUpdated, otherVelocity);
                            laneChangeRecharge = 1f;

                        }
                    }
                    */
                    parRb.AddForce(0, 0, -brakeForce * Time.deltaTime);
                    if (brakeForce > 0)
                    {
                        material_chooser.brakeOn = true;
                    }
                    else
                    {
                        material_chooser.brakeOn = false;
                    }
                    
                }
                else
                {
                    float currentSpeed = parRb.velocity.z;
                    accelerationForce = HitTheGas(currentSpeed, maxSpeed, timeToCollision, distanceBetween);

                    parRb.AddForce(0, 0, accelerationForce * Time.deltaTime);
                        
                }
                /*
                else if (timeToCollision < -2f && parVelocity < maxSpeed)
                {
                    accelerationForce = 100f;
                    parRb.AddForce(0, 0, accelerationForce * Time.deltaTime);
                    material_chooser.brakeOn = false;
                }
                */
                
            }
        }

    }

    public void tooManyObstaclesFunction()
    {
        expensiveFunctCount++;
        Vector3 overlapBoxSize = new Vector3(sensorResizeX / 2, 0.25f, 25f);
        Vector3 centerPosition = parRb.position;
        centerPosition.z += 25.51f;
        centerPosition.x += sensorPosX;

        Collider[] obstacleCheck = Physics.OverlapBox(centerPosition, overlapBoxSize, Quaternion.identity, layermask);

        float shortestDist = 9999f;

        foreach (Collider go in obstacleCheck)
        {
            if (go == parCol)
            {
                //Debug.Log("Check your half extents!!");
            }
            else
            {
                float dist = go.transform.position.z;
                if(dist < shortestDist)
                {
                    shortestDist = dist;
                    otherUpdated = go;
                }

            }
        }

        if (expensiveFunctCount >= 12)
        {
            //Debug.Log(parRb.name + " has called tooManyObstacles() " + expensiveFunctCount + " times.");
            //Debug.Break();
        }
    }

    public void obstacleRespawn()
    {
        otherUpdated = null;
        triggerList = new List<Collider>();
        colpos = new Vector3(0, 0.5f, 25.5f);
        colpos.x = sensorPosX;
        transform.localPosition = colpos;
        transform.localScale = new Vector3(sensorResizeX, 0.5f, 50f);
        expensiveFunctCount = 0;
        maxSpeed = Random.Range(-10f, 0f);

    }

    private float ApplyBrakes(float collisionTime)
    {
        //float brakeForce = (-800f * collisionTime) + 2400f;

        float brakeForcePow = (-0.8f * timeToCollision) + 4.7f;
        float brakeForce = Mathf.Pow(7f, brakeForcePow);

        if (brakeForce < 100)
        {
            brakeForce = 0;
        }

        return brakeForce;
    }

    private float PreventTailgate(float followingDistance)
    {
        float tailgateBrakeForce = 1000f;



        return tailgateBrakeForce;
    }

    private float HitTheGas(float currentSpeed, float maxSpeed, float collisionTime, float followingDistance)
    {
        float accelerationForce = 0;

        if (currentSpeed > maxSpeed || followingDistance < 10f)
        {

        }
        else
        {
            accelerationForce = 750f;
            Debug.DrawRay(parRb.position, Vector3.up * 50f, Color.cyan);
        }

        return accelerationForce;

    }
    private void OnTriggerEnter(Collider other)
    {
        triggerList.Add(other);
        if (triggerList.Count == 1)
        {
            otherUpdated = other;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        triggerList.Remove(other);        
    }
   
}

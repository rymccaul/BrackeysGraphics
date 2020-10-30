using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public Swipe swipeControls;

    public Rigidbody rb;
    public float startpos_x = 0;
    public float startpos_y = 1;
    public float startpos_z = 0;
    public int currentlane = 0;
    public int destinationlane = 0;
    public float pos;
    public float turnspeed;

    public bool carCrash;
    public float carCrashCounter;

    public bool isGrounded;

    public float crashXvelocity;
    public float crashAngularVelocity;

    public bool fixRotation;
    public float fixRotInitialAngle;
    public int restoreTorqueApplied;
    public float restoreTorqueVector;
    public float targetRotation;
    public float currentRot;
    public float priorRot;

    public float xSwerveCurrent;
    public float xSwervePrior;
    public float swerveRot;
    public bool fixPos;

    public bool boostOn;
    public float boostForce;
    public float crashZvelocity;
    public float boostCounter;
    public float boostRecharge;

    // this is the start of the hinge method

    public frontHingeAnchorScript frontHingeScript;
    public backHingeAnchorScript backHingeScript;
    public Rigidbody anchorObjectFront;
    public Rigidbody anchorObjectBack;
    public Vector3 anchorCoordinates;
    public HingeJoint hinge;
    public int hingedestinationlane;
    public bool hingeApplied;
    public bool rotMultiplier;
    public float priorHingeRot;
    public float maxPosHingeRot;
    public float maxNegHingeRot;
    public float currentHingeRot;
    public float targetHingeRot;
    public bool posSwingCheck;
    public bool negSwingCheck;
    public float swerveRecoveryFactor;
    public Vector3 crashResetVel;
    public bool postCrashZvelFix;
    public float restoreSpeed;

    // this is the end of the hinge method

    public float jumpRecharge;
    public float jumpForce;
    public bool jumpOn;
    public Vector3 jumpXSpeedLimit;

    public float blastRecharge;
    public bool blastOn;
    private int layermask;
    public float blastForce;
    public float radius;
    public Vector3 blastCentre;

    // Mobile Touch specific variables
    public bool wantsToBlast;

    private bool wantsToJump;
    private bool wantsToMoveLeft;
    private bool wantsToMoveRight;
    private float screenWidth;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(startpos_x, startpos_y, startpos_z);
        rb.velocity = new Vector3(0, 0, 0);
        turnspeed = 500;

        carCrash = false;
        carCrashCounter = 0.5f;

        fixRotation = false;
        restoreTorqueApplied = 0;
        targetRotation = 0;

        rb.angularDrag = 1.5f;

        currentRot = 0;
        xSwerveCurrent = 0;
        xSwervePrior = 0;
        swerveRecoveryFactor = 3;

        hingeApplied = false;

        boostOn = false;
        boostForce = 500;

        postCrashZvelFix = false;

        restoreSpeed = 100f;

        jumpOn = false;
        jumpRecharge = 0;
        jumpForce = 300;

        blastOn = false;
        blastRecharge = 0;
        int obstacles = 1 << LayerMask.NameToLayer("obstacles");
        layermask = obstacles;
        radius = 5;
        blastForce = 1000;

        screenWidth = Screen.width;
    }

    /*
    void Update()
    {
        if (Input.GetKey("d") && (currentlane == destinationlane))
        {
            destinationlane++;
            rb.AddForce(turnspeed, 0, 0);
        }

        if (Input.GetKey("a") && (currentlane == destinationlane))
        {
            destinationlane--;
            rb.AddForce(-turnspeed, 0, 0);
        }

        if (destinationlane > currentlane)
        {
            pos = transform.position.x;
            if (Mathf.Abs((destinationlane * 2) - pos) < 0.1)
            {
                transform.position = new Vector3((destinationlane * 2), 1, 0);
                rb.AddForce(-turnspeed, 0, 0);
                currentlane = destinationlane;
            }
        }

        if (destinationlane < currentlane)
        {
            pos = transform.position.x;
            if (Mathf.Abs((destinationlane * 2) - pos) < 0.1)
            {
                transform.position = new Vector3((destinationlane * 2), 1, 0);
                rb.AddForce(turnspeed, 0, 0);
                currentlane = destinationlane;
            }
        }
    }

    */
    void Update(){
      // Move variables set to false to after the action has been executed, or
      // for the powerups only if they were ready and executed. Else only set
      // wants


      if (swipeControls.SwipeDown && blastRecharge <= 0)
          wantsToBlast = true;

      if (swipeControls.SwipeUp && boostRecharge <= 0)
          wantsToJump = true;

      if (swipeControls.TapLeft)
          wantsToMoveLeft = true;

      if (swipeControls.TapRight)
          wantsToMoveRight = true;
    }


    // Update is called once per frame

    void FixedUpdate()
    {

        // fix post-crash z velocity
        if (postCrashZvelFix == true)
        {
            rb.velocity = new Vector3(0, 0, crashResetVel.z);
            rb.angularVelocity = Vector3.zero;
            postCrashZvelFix = false;
        }

        if (boostOn == true)
        {
            rb.AddForce(0, 0, (boostForce/2) * Time.deltaTime);
            boostCounter -= Time.deltaTime;
            if (boostCounter < 0)
            {
                boostOn = false;
                boostRecharge = 3f;
            }
        }
        else
        {
            boostRecharge -= Time.deltaTime;
            float currentSpeed = rb.velocity.z;

            // Decay speed back to normal if not currently boosting
            if (Mathf.Abs(currentSpeed) < 0.5f)
            {
                // if close to original speed, stop decaying
                Vector3 resetSpeed = rb.velocity;
                resetSpeed.z = 0;
                rb.velocity = resetSpeed;
            }
            else
            {
                // slowly decay speed down
                if (currentSpeed > 0)
                {
                    rb.AddForce(0, 0, -restoreSpeed * Time.deltaTime);
                }
                else
                {
                    rb.AddForce(0, 0, restoreSpeed * 2 * Time.deltaTime);
                }
            }
        }

        jumpRecharge -= Time.deltaTime;
        if (jumpRecharge < 0)
        {
            jumpOn = false;
        }

        blastRecharge -= Time.deltaTime;
        if (blastRecharge < 0)
        {
            blastOn = false;
        }

        if (Input.GetKey("s") || wantsToBlast)
        {
            if (blastRecharge < 0)
            {
                if (blastOn == false)
                {
                    blastCentre = rb.position;
                    blastCentre.z += 10f;
                    blastOn = true;
                    blastRecharge = 2;
                    wantsToBlast = false;
                    /*
                    Collider[] blastRadius = Physics.OverlapSphere(blastCentre, radius, layermask);
                    foreach (Collider hit in blastRadius)
                    {
                        Rigidbody blastRb = hit.GetComponent<Rigidbody>();
                        blastRb.AddExplosionForce(blastForce, blastCentre, radius, 100f);
                        Debug.Log("We hit " + hit.name);
                    }
                    */
                }
            }
        }

        if (carCrash == true)
        {
            if (isGrounded == true)
            {
                if (rotMultiplier == true)
                {
                    rb.angularVelocity *= 100f;
                    rotMultiplier = false;
                }
                crashAngularVelocity = rb.angularVelocity.y;

                carCrashCounter -= Time.deltaTime;

                if (crashAngularVelocity > 1 || crashAngularVelocity < -1)
                {

                    //rb.AddRelativeTorque(0, -crashAngularVelocity * 20f * Time.deltaTime, 0);

                    /*
                    Vector3 crashVelocityLimiter = rb.velocity;
                    if (crashVelocityLimiter.x > 5)
                    {
                        crashVelocityLimiter.x = 5;
                        rb.velocity = crashVelocityLimiter;
                        Debug.Break();
                    }else if (crashVelocityLimiter.x < -5)
                    {
                        crashVelocityLimiter.x = -5;
                        rb.velocity = crashVelocityLimiter;
                        Debug.Break();
                    }
                    */

                    crashXvelocity = rb.velocity.x;
                    if (crashXvelocity > 5 || crashXvelocity < -5)
                    {
                        rb.AddForce(-crashXvelocity * 100f * Time.deltaTime, 0, 0);
                    }

                    if (Input.GetKey("d") || wantsToMoveRight)
                    {
                        rb.AddForce(turnspeed * Time.deltaTime *1.5f, 0, 0);
                        wantsToMoveRight = false;
                    }

                    if (Input.GetKey("a") || wantsToMoveLeft)
                    {
                        rb.AddForce(-turnspeed * Time.deltaTime * 1.5f, 0, 0);
                        wantsToMoveLeft = false;
                    }
                }
                else
                {
                    float crashPos = transform.position.x;
                    crashXvelocity = rb.velocity.x;

                    fixRotation = true;
                    restoreTorqueApplied = 0;

                    // this is the Hinge method below vvvv

                    if (crashPos < -5)
                    {
                        hingedestinationlane = -3;
                        destinationlane = -3;
                        currentlane = -3;
                    }
                    else if (crashPos < -3)
                    {
                        hingedestinationlane = -2;
                        destinationlane = -2;
                        currentlane = -2;
                    }
                    else if (crashPos < -1)
                    {
                        hingedestinationlane = -1;
                        destinationlane = -1;
                        currentlane = -1;
                    }
                    else if (crashPos < 1)
                    {
                        hingedestinationlane = 0;
                        destinationlane = 0;
                        currentlane = 0;
                    }
                    else if (crashPos < 3)
                    {
                        hingedestinationlane = 1;
                        destinationlane = 1;
                        currentlane = 1;
                    }
                    else if (crashPos < 5)
                    {
                        hingedestinationlane = 2;
                        destinationlane = 2;
                        currentlane = 2;
                    }
                    else if (crashPos > 5)
                    {
                        hingedestinationlane = 3;
                        destinationlane = 3;
                        currentlane = 3;
                    }

                    carCrash = false;
                }

            }

        }
        else
        {
            if (hingeApplied == false)
            {
                if (jumpOn == false)
                {

                    if ((Input.GetKey("d") || (wantsToMoveRight))
                                        && (currentlane == destinationlane))
                    {
                        destinationlane++;
                        rb.AddForce(turnspeed, 0, 0);
                        wantsToMoveRight = false;
                    }

                    if ((Input.GetKey("a") || (wantsToMoveLeft)) && (currentlane == destinationlane))
                    {
                        destinationlane--;
                        rb.AddForce(-turnspeed, 0, 0);
                        wantsToMoveLeft = false;
                    }
                }
                else
                {
                    jumpXSpeedLimit = rb.velocity;

                    if (jumpXSpeedLimit.x > 6)
                    {
                        jumpXSpeedLimit.x = 6;
                        rb.velocity = jumpXSpeedLimit;
                    }else if (jumpXSpeedLimit.x < -6)
                    {
                        jumpXSpeedLimit.x = -6;
                        rb.velocity = jumpXSpeedLimit;
                    }

                    if (Input.GetKey("d") || (wantsToMoveRight))
                    {
                        if (rb.velocity.x < 5)
                        {
                            rb.AddForce(turnspeed * Time.deltaTime * 1.0f, 0, 0);
                        }
                        wantsToMoveRight = false;
                    }

                    if (Input.GetKey("a") || (wantsToMoveLeft))
                    {
                        if (rb.velocity.x > -5)
                        {
                            rb.AddForce(-turnspeed * Time.deltaTime * 1.0f, 0, 0);
                        }
                        wantsToMoveLeft = false;
                    }
                }

                if (Input.GetKey("w") && boostOn == false && boostRecharge <= 0)
                {
                    boostOn = true;
                    boostCounter = 1f;
                    rb.AddForce(0, 0, boostForce/2);
                }

                if ((Input.GetKey(KeyCode.Space) && isGrounded == true && jumpOn == false && jumpRecharge <= 0) ||
                    (wantsToJump && isGrounded == true && jumpOn == false && jumpRecharge <= 0))
                {
                    jumpOn = true;
                    jumpRecharge = 3;
                    Vector3 reduceXvel = rb.velocity;
                    reduceXvel.x *= .25f;
                    rb.velocity = reduceXvel;
                    rb.AddForce(0, jumpForce, 0);
                    wantsToJump = false;
                }

            }
            else
            {
                currentHingeRot = rb.transform.localEulerAngles.y;

                if (currentHingeRot > 180)
                {
                    currentHingeRot -= 360;
                }

                if (targetHingeRot == 180)
                {
                    if (currentHingeRot > 0)
                    {
                        currentHingeRot = 180 - currentHingeRot;
                    }else if (currentHingeRot < 0)
                    {
                        currentHingeRot = -(180 + currentHingeRot);
                    }
                }

                    if (currentHingeRot > 0)
                    {
                        if (currentHingeRot < priorHingeRot)
                        {
                            maxPosHingeRot = priorHingeRot;
                            posSwingCheck = true;
                        }
                    }
                    else if (currentHingeRot < 0)
                    {
                        if (currentHingeRot > priorHingeRot)
                        {
                            maxNegHingeRot = priorHingeRot;
                            negSwingCheck = true;
                        }
                    }

                priorHingeRot = currentHingeRot;

                if (posSwingCheck == true && negSwingCheck == true)
                {
                    if (Mathf.Abs(currentHingeRot) < (0.5f * swerveRecoveryFactor)
                    && maxNegHingeRot > (-0.5f * swerveRecoveryFactor) &&
                        maxPosHingeRot < (0.5f * swerveRecoveryFactor))
                    {
                        Destroy(gameObject.GetComponent<HingeJoint>());
                        hingeApplied = false;
                        frontHingeScript.ReParent();
                        backHingeScript.ReParent();
                        currentlane = hingedestinationlane;
                        destinationlane = currentlane;
                        if (targetHingeRot == 0)
                        {
                            rb.transform.rotation = Quaternion.identity;
                            crashResetVel.z = frontHingeScript.playerZvel;
                            postCrashZvelFix = true;
                        }
                        else
                        {
                            rb.transform.rotation = Quaternion.Euler(0,180,0);
                            crashResetVel.z = backHingeScript.playerZvel;
                            postCrashZvelFix = true;
                        }
                        crashResetVel.x = 0;
                        //rb.velocity = crashResetVel;
                        //rb.AddForce(0, 0, frontHingeScript.playerZvel);
                        rb.angularVelocity = Vector3.zero;
                        boostRecharge = 0f;
                    }
                }
            }

            if (jumpOn == false)
            {


                if (destinationlane > currentlane)
                {
                    pos = transform.position.x;
                    float zpos = transform.position.z;
                    if (Mathf.Abs((destinationlane * 2) - pos + xSwerveCurrent) < 0.1)
                    {
                        transform.position = new Vector3((destinationlane * 2) + xSwerveCurrent, 1, zpos);
                        Vector3 resetVel = rb.velocity;
                        resetVel.x = 0;
                        rb.velocity = resetVel;
                        currentlane = destinationlane;
                    }
                }

                if (destinationlane < currentlane)
                {
                    pos = transform.position.x;
                    float zpos = transform.position.z;
                    if (Mathf.Abs((destinationlane * 2) - pos + xSwerveCurrent) < 0.1)
                    {
                        transform.position = new Vector3((destinationlane * 2) + xSwerveCurrent, 1, zpos);
                        //rb.AddForce(turnspeed, 0, 0); DON'T PUT BACK IN
                        Vector3 resetVel = rb.velocity;
                        resetVel.x = 0;
                        rb.velocity = resetVel;
                        //rb.angularVelocity = new Vector3(0, 0, 0);
                        //rb.rotation = Quaternion.identity;
                        currentlane = destinationlane;
                        //restoreTorqueApplied = 0;
                        //fixRotation = true;

                    }
                }
            }

            if (fixRotation == true && hingeApplied == false && isGrounded == true)
            {
                Vector3 fixpos = rb.transform.position;
                fixpos.y = 1;
                rb.transform.position = fixpos;

                Vector3 fixrot = rb.transform.rotation.eulerAngles;
                fixrot.x = 0;
                fixrot.z = 0;
                rb.transform.rotation = Quaternion.Euler(fixrot);



                if (gameObject.GetComponent<HingeJoint>() != null)
                {
                    Destroy(gameObject.GetComponent<HingeJoint>());
                }
                frontHingeScript.xTarget = hingedestinationlane * 2;
                backHingeScript.xTarget = hingedestinationlane * 2;
                float frontPos = anchorObjectFront.transform.position.z;
                float backPos = anchorObjectBack.transform.position.z;

                gameObject.AddComponent<HingeJoint>();
                hinge = gameObject.GetComponent<HingeJoint>();

                if (frontPos > backPos)
                {
                    anchorCoordinates = anchorObjectFront.transform.position;
                    anchorCoordinates.y = 1;
                    targetHingeRot = 0;
                }
                else
                {
                    anchorCoordinates = anchorObjectBack.transform.position;
                    anchorCoordinates.y = 1;
                    targetHingeRot = 180;
                }

                Vector3 anchorpoint = transform.InverseTransformPoint(anchorCoordinates);

                if (frontPos > backPos)
                {

                    frontHingeScript.ClearParent();
                }
                else
                {
                    backHingeScript.ClearParent();
                }

                hinge.anchor = anchorpoint;
                hinge.axis = new Vector3(0, 1, 0);
                hinge.autoConfigureConnectedAnchor = true;
                JointSpring hingeSpring = hinge.spring;

                if (frontPos > backPos)
                {
                    hinge.connectedBody = anchorObjectFront;
                }
                else
                {
                    hinge.connectedBody = anchorObjectBack;
                }

                hingeSpring.spring = 20000f;
                hingeSpring.damper = 800f;
                hinge.breakForce = 0.1f;
                hinge.spring = hingeSpring;
                hinge.useSpring = true;
                fixRotation = false;
                hingeApplied = true;
                posSwingCheck = false;
                negSwingCheck = false;
                priorHingeRot = 0;
                maxPosHingeRot = 0;
                maxNegHingeRot = 0;
                currentHingeRot = 0;

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        // collision with road
        if (collision.gameObject.layer == 8)
        {
            isGrounded = true;
            if (jumpOn == true)
            {
                if (carCrash == false)
                {
                    swerveRecoveryFactor = 3;
                    crashZvelocity = rb.velocity.z;
                    crashXvelocity = rb.velocity.x;
                    carCrash = true;
                }
                jumpOn = false;
            }
        }

        // collision with obstacles
        if (collision.gameObject.layer == 10)
        {
            Destroy(gameObject.GetComponent<HingeJoint>());
            frontHingeScript.ReParent();
            backHingeScript.ReParent();
            carCrash = false;
            boostOn = false;
            fixRotation = false;
            hingeApplied = false;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isGrounded = false;
        }

        if (collision.gameObject.layer == 10)
        {
            //Destroy(gameObject.GetComponent<HingeJoint>());
            carCrash = true;
            fixRotation = false;
            if (hingeApplied == true)
            {
                rotMultiplier = true;
            }
            hingeApplied = false;
            restoreTorqueApplied = 0;
            targetRotation = 0;
            carCrashCounter = 0.5f;
            crashXvelocity = rb.velocity.x;
            //Debug.Log("Player's crash velocity (x) is: " + crashXvelocity);
            xSwerveCurrent = 0;
            xSwervePrior = 0;
            swerveRecoveryFactor = 2;

            crashZvelocity = rb.velocity.z;
            //Debug.Log("Player's crash velocity (z) is: " + crashZvelocity);

            //Debug.Break();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frontHingeAnchorScript : MonoBehaviour
{
    public GameObject player;
    public Rigidbody rb;
    public Rigidbody playerRb;
    public Rigidbody anchorMoveRb;
    public bool letsMove;
    public float xTarget;
    public int leftOrRight;
    public Player_movement playerMovementScript;
    public int hingeLane;
    public bool readyForInput;

    public float playerZvel;
    public Vector3 anchorVelocity;
    public anchorMoverScript anchorMovementScript;
    public int slowOrFastCrash;
    public Vector3 target;
    public Vector3 targetWorldSpace;

    public float xMovementCoordinate;
    public bool xMove;
    public float zMovementCoordinate;
    public bool zMove;

    public float anchorRot;
    public int clockwiseOrAnti;

    public float xMoveMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        letsMove = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (letsMove == true)
        {
            Vector3 pos = rb.transform.position;
            float playerZpos = player.transform.position.z;
            //rb.transform.rotation = Quaternion.identity;

            anchorRot = rb.rotation.eulerAngles.y;

            if (anchorRot > 180)
            {
                anchorRot -= 360;
            }
            else if (anchorRot < -180)
            {
                anchorRot += 360;
            }

            if (clockwiseOrAnti == 1)
            {
                if (anchorRot < 0)
                {
                    rb.MoveRotation(Quaternion.Euler(0, (anchorRot + (Time.deltaTime * 180)), 0));
                    //rb.transform.rotation = (Quaternion.Euler(0, (anchorRot - (Time.deltaTime * 180 * 0.6f)), 0));
                }
                else
                {
                    clockwiseOrAnti = 0;
                }
            }
            else if (clockwiseOrAnti == -1)
            {
                if (anchorRot > 0)
                {
                    rb.MoveRotation(Quaternion.Euler(0, (anchorRot - (Time.deltaTime * 180)), 0));
                    //rb.transform.rotation = (Quaternion.Euler(0, (anchorRot + (Time.deltaTime * 180 * 0.6f)), 0));
                }
                else
                {
                    clockwiseOrAnti = 0;
                }

            }
            else if (clockwiseOrAnti == 0)
            {
                rb.transform.rotation = Quaternion.identity;
            }


            if (Input.GetKey("d") && readyForInput == true)
            {
                hingeLane = playerMovementScript.hingedestinationlane;
                hingeLane++;
                playerMovementScript.hingedestinationlane = hingeLane;
                xTarget = 2 * hingeLane;
                xMoveMultiplier = 10;
                leftOrRight = 1;
                readyForInput = false;
                playerMovementScript.swerveRecoveryFactor = 3;
            }

            if (Input.GetKey("a") && readyForInput == true)
            {
                hingeLane = playerMovementScript.hingedestinationlane;
                hingeLane--;
                playerMovementScript.hingedestinationlane = hingeLane;
                xTarget = 2 * hingeLane;
                xMoveMultiplier = 10;
                leftOrRight = -1;
                readyForInput = false;
                playerMovementScript.swerveRecoveryFactor = 3;
            }

            // move left or right by setting xMovementCoordinate and using rb.MovePosition later
            if (leftOrRight == 1)
            {
                if (pos.x < xTarget)
                {
                    xMove = true;
                    xMovementCoordinate = xMoveMultiplier * Time.deltaTime;//(xTarget - pos.x) * Time.deltaTime;
                }
                else
                {
                    xMove = false;
                    xMovementCoordinate = 0;
                    readyForInput = true;
                }
            }
            else if (leftOrRight == -1)
            {
                if (pos.x > xTarget)
                {
                    xMove = true;
                    xMovementCoordinate = -xMoveMultiplier * Time.deltaTime;//(xTarget - pos.x) * Time.deltaTime;
                }
                else
                {
                    xMove = false;
                    xMovementCoordinate = 0;
                    readyForInput = true;
                }
            }

            // move forwards or back by setting zMovementCoordinate and using rb.MovePosition later
            if (slowOrFastCrash == -1)
            {
                if (playerZvel < 0)
                {
                    zMove = true;
                    zMovementCoordinate = playerZvel * Time.deltaTime;
                    playerZvel += 0.2f;
                }
                else
                {
                    playerZvel = 0;
                    zMovementCoordinate = 0;
                    zMove = false;
                }
            }
            else
            {
                if (playerZvel > 0)
                {
                    zMove = true;
                    zMovementCoordinate = playerZvel * Time.deltaTime;
                    playerZvel -= 0.2f;
                }
                else
                {
                    playerZvel = 0;
                    zMovementCoordinate = 0;
                    zMove = false;
                }
            }

            // now we use rb.MovePosition from "later"
            if (xMove == false)
            {
                target = rb.position;
                target.x = xTarget;
                rb.position = target;
            }
            if (zMove == true || xMove == true)
            {
                target = rb.position;
                target.x += xMovementCoordinate;
                target.z += zMovementCoordinate;
                rb.MovePosition(target);
            }

            

            /*
            // move left or right with interpolated rb.MovePosition
            if (leftOrRight == 1)
            {
                if (pos.x < xTarget)
                {
                    target = rb.position;
                    target.x += xTarget * Time.deltaTime;
                    rb.MovePosition(target);

                }
                else
                {
                    pos.x = xTarget;
                    rb.transform.position = pos;
                    readyForInput = true;
                }
            }
            else if (leftOrRight == -1)
            {
                if (pos.x > xTarget)
                {
                    target = rb.position;
                    target.x -= xTarget * Time.deltaTime;
                    rb.MovePosition(target);
                }
                else
                {
                    pos.x = xTarget;
                    rb.transform.position = pos;
                    readyForInput = true;
                }
            }
            */

            
            // move left or right by setting x position directly
            /*
            if (leftOrRight == 1)
            {
                if (pos.x < xTarget)
                {
                    pos.x += Time.deltaTime * 8;
                    rb.transform.position = pos;

                }
                else
                {
                    pos.x = xTarget;
                    rb.transform.position = pos;
                    readyForInput = true;
                }
            }
            else if (leftOrRight == -1)
            {
                if (pos.x > xTarget)
                {
                    pos.x -= Time.deltaTime * 8;
                    rb.transform.position = pos;
                }
                else
                {
                    pos.x = xTarget;
                    rb.transform.position = pos;
                    readyForInput = true;
                }
            }
            */
            
            // move backwards or forwards with interpolated rb.MovePosition
            /*
            if (slowOrFastCrash == -1)
            {
                if (playerZvel < 0)
                {
                    Vector3 slowDownTarget = rb.position;
                    slowDownTarget.z += playerZvel * Time.deltaTime;
                    rb.MovePosition(slowDownTarget);
                    playerZvel += 0.2f;

                }
                else
                {
                    playerZvel = 0;
                }
                rb.transform.rotation = Quaternion.identity;
            }
            else
            {
                if (playerZvel > 0)
                {
                    Vector3 speedUpTarget = rb.position;
                    speedUpTarget.z += playerZvel * Time.deltaTime;
                    rb.MovePosition(speedUpTarget);
                    playerZvel -= 0.2f;
                }
                else
                {
                    playerZvel = 0;
                }
                rb.transform.rotation = Quaternion.identity;
            }
            */

            /*
            if (Mathf.Abs(rot) < 5)
            {
                rb.transform.rotation = Quaternion.identity;
            }
            else
            {
                Vector3 rotation = rb.transform.eulerAngles;
                rotation.y -= 45 * Time.deltaTime;
                rb.transform.rotation = Quaternion.Euler(rotation);
            }
            */
        }
    }

    public void ClearParent()
    {
        //Debug.Break();
        transform.parent = null;
        playerZvel = playerMovementScript.crashZvelocity;

        if (playerZvel < 0)
        {
            slowOrFastCrash = -1;
        }
        else
        {
            slowOrFastCrash = 1;
        }

        if (rb.transform.position.x > player.transform.position.x)
        {
            leftOrRight = -1;
            clockwiseOrAnti = -1;
        }
        else
        {
            leftOrRight = 1;
            clockwiseOrAnti = 1;
        }

        xMoveMultiplier = 6;
        readyForInput = false;
        letsMove = true;
    }

    public void ReParent()
    {

        letsMove = false;
        //rb.transform.SetParent(player.transform);
        //rb.velocity = new Vector3(0, 0, playerZvel / 3);
        rb.angularVelocity = Vector3.zero;

        transform.position = player.transform.position + (player.transform.forward * 3f);
        transform.rotation = player.transform.rotation;

        rb.transform.SetParent(player.transform);

        

        //Vector3 resetPlayerVel = playerRb.velocity;
        //resetPlayerVel.z = playerZvel;
        //playerRb.velocity = resetPlayerVel;


        //transform.position = player.transform.forward * 1f;

        //transform.localPosition = new Vector3(0, 0, 4);
        //transform.localRotation = Quaternion.identity;

        //Vector3 resetPos = transform.TransformPoint(Vector3.forward * .25f);
        //rb.transform.position = resetPos;
    }
}

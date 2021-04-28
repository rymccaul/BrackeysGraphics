using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backHingeAnchorScript : MonoBehaviour
{
    public GameObject player;
    public Rigidbody rb;
    public bool letsMove;
    public float xTarget;
    public int leftOrRight;
    public Player_movement playerMovementScript;
    public int hingeLane;
    public bool readyForInput;

    public float playerZvel;
    public Vector3 anchorVelocity;
    public int slowOrFastCrash;
    public Vector3 target;
    public Vector3 targetWorldSpace;

    public float xMovementCoordinate;
    public bool xMove;
    public float zMovementCoordinate;
    public bool zMove;

    public float anchorRot;
    //public float targetRotPos;
    //public float targetRotNeg;
    public int clockwiseOrAnti;

    public float xMoveMultiplier;

    private Player_movement playerScript;

    // Start is called before the first frame update
    void Start()
    {
        letsMove = false;
        playerScript = player.GetComponent<Player_movement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (letsMove == true)
        {
            Vector3 pos = rb.transform.position;
            float playerZpos = player.transform.position.z;
            //rb.transform.rotation = Quaternion.Euler(0, 180, 0);
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
                if (anchorRot > 0)
                {
                    rb.MoveRotation(Quaternion.Euler(0, (anchorRot + (Time.deltaTime * 180)), 0));
                    //rb.transform.rotation = (Quaternion.Euler(0, (anchorRot + (Time.deltaTime * 180 * 0.6f)), 0));
                }
                else
                {
                    clockwiseOrAnti = 0;
                }
            }
            else if (clockwiseOrAnti == -1)
            {
                if (anchorRot < 0)
                {
                    rb.MoveRotation(Quaternion.Euler(0, (anchorRot - (Time.deltaTime * 180)), 0));
                    //rb.transform.rotation = (Quaternion.Euler(0, (anchorRot - (Time.deltaTime * 180 * 0.6f)), 0));
                }
                else
                {
                    clockwiseOrAnti = 0;
                }

            }
            else if (clockwiseOrAnti == 0)
            {
                rb.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            if ((Input.GetKey("d") || playerScript.holdingRight) && readyForInput == true)
            {
                hingeLane = playerMovementScript.hingedestinationlane;
                hingeLane++;
                playerMovementScript.hingedestinationlane = hingeLane;
                xTarget =  2 * hingeLane;
                xMoveMultiplier = 10;
                leftOrRight = 1;
                readyForInput = false;
                //playerMovementScript.swerveRecoveryFactor = 3f;
            }

            if ((Input.GetKey("a") || playerScript.holdingLeft) && readyForInput == true)
            {
                hingeLane = playerMovementScript.hingedestinationlane;
                hingeLane--;
                playerMovementScript.hingedestinationlane = hingeLane;
                xTarget =  2 * hingeLane;
                xMoveMultiplier = 10;
                leftOrRight = -1;
                readyForInput = false;
                //playerMovementScript.swerveRecoveryFactor = 3f;
            }

            /*
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



            if (leftOrRight == 1)
            {
                if (pos.x < xTarget)
                {
                    xMove = true;
                    xMovementCoordinate = xMoveMultiplier * Time.deltaTime;
                    //pos.x += Time.deltaTime * 8;
                    //rb.transform.position = pos;
                }
                else
                {
                    xMove = false;
                    xMovementCoordinate = 0;
                    readyForInput = true;
                    //pos.x = xTarget;
                    //rb.transform.position = pos;
                    //readyForInput = true;
                }
            }
            else if (leftOrRight == -1)
            {
                if (pos.x > xTarget)
                {
                    xMove = true;
                    xMovementCoordinate = -xMoveMultiplier * Time.deltaTime;
                    //pos.x -= Time.deltaTime * 8;
                    //rb.transform.position = pos;
                }
                else
                {
                    xMove = false;
                    xMovementCoordinate = 0;
                    readyForInput = true;
                    //pos.x = xTarget;
                    //rb.transform.position = pos;
                    //readyForInput = true;
                }
            }


            if (slowOrFastCrash == -1)
            {
                if (playerZvel < 0)
                {
                    zMove = true;
                    zMovementCoordinate = playerZvel * Time.deltaTime;
                    playerZvel += 0.2f;
                    //Vector3 slowDownTarget = rb.position;
                    //slowDownTarget.z += playerZvel * Time.deltaTime;
                    //rb.MovePosition(slowDownTarget);
                    //playerZvel += 0.2f;
                }
                else
                {
                    playerZvel = 0;
                    zMovementCoordinate = 0;
                    zMove = false;
                    //playerZvel = 0;
                }
                //rb.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                if (playerZvel > 0)
                {
                    zMove = true;
                    zMovementCoordinate = playerZvel * Time.deltaTime;
                    playerZvel -= 0.2f;
                    //Vector3 speedUpTarget = rb.position;
                    //speedUpTarget.z += playerZvel * Time.deltaTime;
                    //rb.MovePosition(speedUpTarget);
                    //playerZvel -= 0.2f;
                }
                else
                {
                    playerZvel = 0;
                    zMovementCoordinate = 0;
                    zMove = false;
                    //playerZvel = 0;
                }
                //rb.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

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
            anchorRot = rb.rotation.eulerAngles.y;

            if (anchorRot > 180)
            {
                anchorRot -= 360;
            }else if (anchorRot < -180)
            {
                anchorRot += 360;
            }

            if (clockwiseOrAnti == 1)
            {
                if (anchorRot > 0)
                {
                    rb.MoveRotation(Quaternion.Euler(0, (anchorRot + (Time.deltaTime * 180)), 0));
                }
                else
                {
                    clockwiseOrAnti = 0;
                }
            }
            else if (clockwiseOrAnti == -1)
            {
                if (anchorRot < 0)
                {
                    rb.MoveRotation(Quaternion.Euler(0, (anchorRot - (Time.deltaTime * 180)), 0));
                }
                else
                {
                    clockwiseOrAnti = 0;
                }

            }
            else if (clockwiseOrAnti == 0)
            {
                rb.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            */

            //targetRotPos = anchorRot + (Time.deltaTime * 180);
            //targetRotNeg = anchorRot - (Time.deltaTime * 180);

            //Debug.Log("Anchor rotation is " + anchorRot);
            //Debug.Log("+ gives us " + targetRotPos + " , - gives us " + targetRotNeg);
            //Debug.Break();

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
        //rb.transform.SetParent(player.transform);
        letsMove = false;

        //rb.velocity = new Vector3(0, 0, playerZvel / 3);
        rb.angularVelocity = Vector3.zero;

        transform.position = player.transform.position + (player.transform.forward * -5f);
        transform.rotation = player.transform.rotation;

        rb.transform.SetParent(player.transform);



        //transform.position = player.transform.forward * -1f;

        //transform.localPosition = new Vector3(0, 0, -4);
        //transform.localRotation = Quaternion.identity;


        //Vector3 resetPos = transform.TransformPoint(Vector3.forward * -.25f);
        //rb.transform.position = resetPos;
    }
}

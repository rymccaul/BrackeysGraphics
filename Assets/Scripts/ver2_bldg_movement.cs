using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ver2_bldg_movement : MonoBehaviour
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

    public GameObject bldg1;
    public GameObject bldg2;
    public GameObject bldg3;
    public GameObject bldg4;
    public GameObject bldg5;
    public GameObject bldg6;
    public GameObject bldg7;
    public GameObject bldg8;
    public GameObject bldg9;
    public GameObject bldg10;
    public GameObject bldg11;
    public GameObject bldg12;
    public GameObject bldg13;
    public GameObject bldg14;

    private int numberOfBuildingTypes;
    private string debugReshuffle;

    private GameObject[] bldgArray;
    private float[] bldgWidthArray;
    public float widthMultiplier;

    private Vector3 emptyRootPosition;
    private GameObject bldgToPlace;
    private float bldgToPlaceWidth;


    // Start is called before the first frame update
    void Start()
    {
        bldgSpeed = new Vector3(0, 0, -25f);
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();

        
        transform.localScale = new Vector3(30, 30, 30);
       
        Quaternion startingRotation = Quaternion.Euler(0, 180f, 0);
        transform.rotation = startingRotation;
        defaultXpos = -33.5f;
        respawnXpos = -58.8f;
        actuallyMoving = true;
        scaleIncrease = 1f;
        startingScale = 0.5f;
        posIncrease = scaleIncrease / (30 * (1 - startingScale));

        numberOfBuildingTypes = 14;

        bldgArray = new GameObject[numberOfBuildingTypes];

        bldgArray[0] = bldg1;
        bldgArray[1] = bldg2;
        bldgArray[2] = bldg3;
        bldgArray[3] = bldg4;
        bldgArray[4] = bldg5;
        bldgArray[5] = bldg6;
        bldgArray[6] = bldg7;
        bldgArray[7] = bldg8;
        bldgArray[8] = bldg9;
        bldgArray[9] = bldg10;
        bldgArray[10] = bldg11;
        bldgArray[11] = bldg12;
        bldgArray[12] = bldg13;
        bldgArray[13] = bldg14;

        bldgWidthArray = new float[numberOfBuildingTypes];
        bldgWidthArray[0] = 0.087f;
        bldgWidthArray[1] = 0.112f;
        bldgWidthArray[2] = 0.094f;
        bldgWidthArray[3] = 0.128f;
        bldgWidthArray[4] = 0.109f;
        bldgWidthArray[5] = 0.129f;
        bldgWidthArray[6] = 0.129f;
        bldgWidthArray[7] = 0.157f;
        bldgWidthArray[8] = 0.126f;
        bldgWidthArray[9] = 0.177f;
        bldgWidthArray[10] = 0.138f;
        bldgWidthArray[11] = 0.123f;
        bldgWidthArray[12] = 0.141f;
        bldgWidthArray[13] = 0.213f;
        widthMultiplier = 30f;

        ReshuffleBldgs(widthMultiplier*1.5f);
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

        ReshuffleBldgs(widthMultiplier);

        //return randomSign;
    }

    void ReshuffleBldgs(float buildingWidthSpacing)
    {
        List<int> bldgNumber = new List<int>();
        int rand;
        debugReshuffle = "";

        for (int a = 0; a < numberOfBuildingTypes; a++)
        {
            rand = Random.Range(0, 14);

            while (bldgNumber.Contains(rand))
            {
                rand = Random.Range(0, 14);
            }

            bldgNumber.Add(rand);
            debugReshuffle += rand + ", ";
        }

        //Debug.Log("The shuffled list is: " + debugReshuffle);

        

        emptyRootPosition = transform.position;
        //Vector3 offset = new Vector3(-10, 0, 0);
        //Vector3 previousOffset = new Vector3(0, 0, 0);

        for (int a = 0; a < numberOfBuildingTypes; a++)
        {
            int b = bldgNumber[a];
            //GameObject bldgToPlace;
            //float bldgToPlaceWidth;
            //Debug.Log("Swtich statement bldg = " + bldgNumber[a]);

            switch (b)
            {
                case 0:
                    bldgToPlace = bldg1;
                    bldgToPlaceWidth = bldgWidthArray[0];
                    break;
                case 1:
                    bldgToPlace = bldg2;
                    bldgToPlaceWidth = bldgWidthArray[1];
                    break;
                case 2:
                    bldgToPlace = bldg3;
                    bldgToPlaceWidth = bldgWidthArray[2];
                    break;
                case 3:
                    bldgToPlace = bldg4;
                    bldgToPlaceWidth = bldgWidthArray[3];
                    break;
                case 4:
                    bldgToPlace = bldg5;
                    bldgToPlaceWidth = bldgWidthArray[4];
                    break;
                case 5:
                    bldgToPlace = bldg6;
                    bldgToPlaceWidth = bldgWidthArray[5];
                    break;
                case 6:
                    bldgToPlace = bldg7;
                    bldgToPlaceWidth = bldgWidthArray[6];
                    break;
                case 7:
                    bldgToPlace = bldg8;
                    bldgToPlaceWidth = bldgWidthArray[7];
                    break;
                case 8:
                    bldgToPlace = bldg9;
                    bldgToPlaceWidth = bldgWidthArray[8];
                    break;
                case 9:
                    bldgToPlace = bldg10;
                    bldgToPlaceWidth = bldgWidthArray[9];
                    break;
                case 10:
                    bldgToPlace = bldg11;
                    bldgToPlaceWidth = bldgWidthArray[10];
                    break;
                case 11:
                    bldgToPlace = bldg12;
                    bldgToPlaceWidth = bldgWidthArray[11];
                    break;
                case 12:
                    bldgToPlace = bldg13;
                    bldgToPlaceWidth = bldgWidthArray[12];
                    break;
                case 13:
                    bldgToPlace = bldg14;
                    bldgToPlaceWidth = bldgWidthArray[13];
                    break;
                default:
                    Debug.Log("Switch statement error...");
                    break;
            }

            //Debug.Log("Building to place is: " + bldgToPlace.name + ", width = " + bldgToPlaceWidth);

            emptyRootPosition.x -= bldgToPlaceWidth * buildingWidthSpacing * 1.8f;
            bldgToPlace.transform.position = emptyRootPosition;
            emptyRootPosition.x -= bldgToPlaceWidth * buildingWidthSpacing * 1.8f;
            //Debug.Log("Offset was:" + offset.x);
        }

    

    }
}
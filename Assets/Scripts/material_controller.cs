using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class material_controller : MonoBehaviour
{

    public int leftOrRight;
    public bool brakeOn;
    //public Material mat;
    public Material defaultMat;
    public Material left;
    public Material right;
    public Material left_b;
    public Material right_b;
    public Material brakeMat;

    private int brake = 0;

    // Start is called before the first frame update
    void Start()
    {
        leftOrRight = 0;        // use -1 to represent left blinker, 0 for no blinker, 1 for right blinker
        brakeOn = false;        // brakeOn bool will let us know whether to use brakelights or not
                                // we will read whether we changing lanes from obstacle_movement.cs
                                // we will read whether we are braking from sensor_script.cs

        GetComponent<Renderer>().material = defaultMat;
    }

    // Update is called once per frame
    void Update()
    {
        if (brakeOn == false)
        {
            if (leftOrRight == -1)
            {
                GetComponent<Renderer>().material = left;
            }
            else if (leftOrRight == 0)
            {
                GetComponent<Renderer>().material = defaultMat;
            }
            else
            {
                GetComponent<Renderer>().material = right;
            }
        }
        else
        {
            if (leftOrRight == -1)
            {
                GetComponent<Renderer>().material = left_b;
            }
            else if (leftOrRight == 0)
            {
                GetComponent<Renderer>().material = brakeMat;
            }
            else
            {
                GetComponent<Renderer>().material = right_b;
            }
        }
    }

}

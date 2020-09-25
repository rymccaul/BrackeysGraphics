using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class material_controller : MonoBehaviour
{

    public int leftOrRight;
    public bool brakeOn;
    public Material mat;

    private int brake = 0;

    // Start is called before the first frame update
    void Start()
    {
        leftOrRight = 0;        // use -1 to represent left blinker, 0 for no blinker, 1 for right blinker
        brakeOn = false;        // brakeOn bool will let us know whether to use brakelights or not
                                // we will read whether we changing lanes from obstacle_movement.cs
                                // we will read whether we are braking from sensor_script.cs

        mat = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (brakeOn == false) brake = 0; else brake = 2;

        switch(leftOrRight)
        {
            case -1:
                set_material(2, 0, brake);
                break;
            case 1:
                set_material(0, 2, brake);
                break;
            default:
                set_material(0, 0, brake);
                break;
        }
    }

    private void set_material(int ind_l, int ind_r, int brake){
        mat.SetFloat("Vector1_44AB5134", ind_l);
        mat.SetFloat("Vector1_1FC30E3", ind_r);
        mat.SetFloat("Vector1_B1DA328B", brake);
    }
}

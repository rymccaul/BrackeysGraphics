using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class material_controller : MonoBehaviour
{

    public int leftOrRight;
    public bool brakeOn;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        leftOrRight = 0;        // use -1 to represent left blinker, 0 for no blinker, 1 for right blinker
        brakeOn = false;        // brakeOn bool will let us know whether to use brakelights or not
                                // we will read whether we changing lanes and/or braking from obstacle_movement.cs

        mat = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (brakeOn == false)
        {
            if (leftOrRight == -1)
            {
                mat.SetFloat("Vector1_44AB5134", 2);
                mat.SetFloat("Vector1_1FC30E3", 0);
                mat.SetFloat("Vector1_B1DA328B", 0);
                /*
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_44AB5134", 2);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_FC30E3", 0);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_B1DA328B", 0);
                */
                // left blinker ON , brakelights OFF
            }
            else if (leftOrRight == 1)
            {
                mat.SetFloat("Vector1_44AB5134", 0);
                mat.SetFloat("Vector1_1FC30E3", 2);
                mat.SetFloat("Vector1_B1DA328B", 0);
                /*
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_44AB5134", 0);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_FC30E3", 2);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_B1DA328B", 0);
                */
                // right blinker ON , brakelights OFF
            }
            else
            {
                mat.SetFloat("Vector1_44AB5134", 0);
                mat.SetFloat("Vector1_1FC30E3", 0);
                mat.SetFloat("Vector1_B1DA328B", 0);
                /*
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_44AB5134", 0);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_FC30E3", 0);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_B1DA328B", 0);
                */
                // blinkers OFF, brakelights OFF
            }
        }
        else
        {
            if (leftOrRight == -1)
            {
                mat.SetFloat("Vector1_44AB5134", 2);
                mat.SetFloat("Vector1_1FC30E3", 0);
                mat.SetFloat("Vector1_B1DA328B", 2);
                /*
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_44AB5134", 2);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_FC30E3", 0);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_B1DA328B", 2);
                */
                // left blinker ON, brakelights ON
            }
            else if (leftOrRight == 1)
            {
                mat.SetFloat("Vector1_44AB5134", 0);
                mat.SetFloat("Vector1_1FC30E3", 2);
                mat.SetFloat("Vector1_B1DA328B", 2);
                /*
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_44AB5134", 0);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_FC30E3", 2);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_B1DA328B", 2);
                */
                // right blinker ON, brakelights ON
            }
            else
            {
                mat.SetFloat("Vector1_44AB5134", 0);
                mat.SetFloat("Vector1_1FC30E3", 0);
                mat.SetFloat("Vector1_B1DA328B", 2);
                /*
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_44AB5134", 0);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_FC30E3", 0);
                gameObject.GetComponent<Renderer>().material.SetFloat("Vector1_B1DA328B", 2);
                */
                // blinkers OFF, brakelights ON
            }
        }
    }
}

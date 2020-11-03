using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_jump_bar : MonoBehaviour
{
    public float yscale;
    public float blastPercentage;
    public blastScript blasterscript;
    // Start is called before the first frame update
    void Start()
    {
        blastPercentage = 100f;
        yscale = 105f;
        transform.localPosition = new Vector3(0, -93.9f, 0);
        transform.localScale = new Vector3(160, yscale, 1);
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void LateUpdate()
    {
      //  blastPercentage = blasterscript.blastRecharge;
        if (blastPercentage < 0)
        {
            blastPercentage = 1;
        }
        else
        {
            blastPercentage = (blastPercentage / 2);
        }
        yscale = 10 + (173f * (blastPercentage));
        transform.localScale = new Vector3(160, yscale, 1);
    }
}

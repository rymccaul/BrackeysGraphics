using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_boost_bar : MonoBehaviour
{
    public float yscale;
    public float boostPercentage;
    public Player_movement playerScript;

    // Start is called before the first frame update
    void Start()
    {
        boostPercentage = 100f;
        yscale = 92.2f;
        transform.localPosition = new Vector3(1.2f, -91.5f, 0);
        transform.localScale = new Vector3(92.6f, yscale, 1);
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        boostPercentage = playerScript.boostBar;
        yscale = 10.7f + (81.5f * (boostPercentage * .01f));
        transform.localScale = new Vector3(92.6f, yscale, 1);
    }
}

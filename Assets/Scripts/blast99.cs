using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blast99 : MonoBehaviour
{
    // Start is called before the first frame update
    public float blastPercentage;
    public Player_movement blastScript;
    public SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        blastPercentage = blastScript.blastRecharge;

        if (blastPercentage > 0)
        {
            spriteRender.enabled = false;
        }
        else
        {
            spriteRender.enabled = true;
        }
    }
}

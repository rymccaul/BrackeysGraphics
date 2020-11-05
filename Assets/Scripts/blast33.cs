using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blast33 : MonoBehaviour
{
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

        if (blastPercentage > 1.32f)
        {
            spriteRender.enabled = false;
        }
        else
        {
            spriteRender.enabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump_bar_75 : MonoBehaviour
{
    public float jumpPercentage;
    public Player_movement playerScript;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        jumpPercentage = playerScript.jumpRecharge;

        if (jumpPercentage > 0.75f)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }
}

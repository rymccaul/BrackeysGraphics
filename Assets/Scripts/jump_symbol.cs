using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump_symbol : MonoBehaviour
{

    public Player_movement playerScript;
    public float jumpPercentage;
    public SpriteRenderer spriteRender;
    private float alpha;
    private Color white;
    // Start is called before the first frame update
    void Start()
    {
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
        white = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        jumpPercentage = playerScript.jumpRecharge;
        if (jumpPercentage > 0)
        {
            alpha = Mathf.Sin(Time.time * 15f);
            spriteRender.color = new Color(1, 1, 1, alpha);
        }
        else
        {
            spriteRender.color = white; 
        }
    }
}

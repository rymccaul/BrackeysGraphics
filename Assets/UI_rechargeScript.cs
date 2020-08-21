using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_rechargeScript : MonoBehaviour
{
    public Text recharge_text;

    private GameObject player_obj = null;
    private Player_movement player_movement_script;

    private float boost_recharge;
    private float jump_recharge;
    private float blast_recharge;

    private string boost_text;
    private string jump_text;
    private string blast_text;

    // Start is called before the first frame update
    void Start()
    {
        if (player_obj == null)
             player_obj = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        player_movement_script = player_obj.GetComponent<Player_movement>();

        boost_recharge = player_movement_script.boostRecharge;
        jump_recharge = player_movement_script.jumpRecharge;
        blast_recharge = player_movement_script.blastRecharge;

        if(boost_recharge <= 0){
            // Boost ready, set to zero to prevent negative and change color of text
            boost_recharge = 0;
            boost_text = "<color=#000000>Boost: " + boost_recharge.ToString("F2") + "</color>\n";
        }else{
            // Boost not ready, set color back to red
            boost_text = "<color=#FF3232>Boost: " + boost_recharge.ToString("F2") + "</color>\n";
        }

        if(jump_recharge <= 0){
            // jump ready, set to zero to prevent negative and change color of text
            jump_recharge = 0;
            jump_text = "<color=#000000>jump: " + jump_recharge.ToString("F2") + "</color>\n";
        }else{
            // jump not ready, set color back to red
            jump_text = "<color=#FF3232>jump: " + jump_recharge.ToString("F2") + "</color>\n";
        }

        if(blast_recharge <= 0){
            // Blast ready, set to zero to prevent negative and change color of text
            blast_recharge = 0;
            blast_text = "<color=#000000>Blast: " + blast_recharge.ToString("F2") + "</color>";
        }else{
            // Blast not ready, set color back to red
            blast_text = "<color=#FF3232>Blast: " + blast_recharge.ToString("F2") + "</color>";
        }

        recharge_text.text = boost_text + jump_text + blast_text;
    }
}

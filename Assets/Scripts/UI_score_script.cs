using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_score_script : MonoBehaviour
{
    public float current_score = 0;

    private TextMeshProUGUI score_text = null;
    private GameObject speed_text_obj = null;
    //private GameObject time_text_obj = null;

    private UI_speedScript speed_script;
    private float player_speed;

    // Start is called before the first frame update
    void Start()
    {
        speed_text_obj = GameObject.Find("UI_speed_text");
        score_text = GameObject.Find("UI_score").
                                        GetComponent<TMPro.TextMeshProUGUI>();
        speed_script = speed_text_obj.GetComponent<UI_speedScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player_speed = speed_script.player_speed;

        current_score += calculateScoreUpdate(player_speed, 0);
        score_text.text = (current_score).ToString("#");
    }

    // To be used later for more complicated high score calculation
    private float calculateScoreUpdate(float current_speed, float current_time){
        return current_speed;
    }
}

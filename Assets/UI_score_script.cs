using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_score_script : MonoBehaviour
{
    public float current_score = 0;

    private TextMeshProUGUI score_text = null;
    private GameObject speed_text_obj = null;
    private GameObject time_text_obj = null;

    private float player_speed;

    // Start is called before the first frame update
    void Start()
    {
        speed_text_obj = GameObject.Find("UI_speed_text");
        score_text = GameObject.Find("UI_score").
                                        GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        player_speed = speed_text_obj.GetComponent<UI_speedScript>()
                                                                .player_speed;

        current_score += calculateScoreUpdate(player_speed, 0);
        score_text.text = (current_score).ToString("F1");
    }

    private float calculateScoreUpdate(float current_speed, float current_time){
        return current_speed;
    }
}

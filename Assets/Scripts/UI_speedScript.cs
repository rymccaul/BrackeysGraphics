using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_speedScript : MonoBehaviour
{
    public const float ROAD_BACKWARDS_SPEED = 50;
    private Rigidbody player_rb = null;

    private TextMeshProUGUI speed_text = null;
    public float player_speed;

    // Start is called before the first frame update
    void Start()
    {
        if (player_rb == null)
             player_rb = GameObject.FindGameObjectWithTag("Player")
                                                    .GetComponent<Rigidbody>();

        speed_text = GameObject.Find("UI_speed_text").
                                        GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // 50 is the speed of the road moving backwards
        player_speed = player_rb.velocity.z + ROAD_BACKWARDS_SPEED;

        speed_text.text = "SPEED | " + System.String.Format("{0:0.00}",
                                                (player_speed).ToString("F2"));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_speedScript : MonoBehaviour
{
    public const float ROAD_BACKWARDS_SPEED = 50;
    private Rigidbody player_rb = null;

    public Text speed_text;

    // Start is called before the first frame update
    void Start()
    {
        if (player_rb == null)
             player_rb = GameObject.FindGameObjectWithTag("Player")
                                                    .GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 50 is the speed of the road moving backwards
        speed_text.text = "SPEED: " + System.String.Format("{0:0.00}",
                (player_rb.velocity.z + ROAD_BACKWARDS_SPEED).ToString("F2"));
    }
}

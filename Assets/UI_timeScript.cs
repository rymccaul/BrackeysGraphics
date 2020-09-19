using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_timeScript : MonoBehaviour
{
    public Text time_text;
    public float play_time;

    private float start_time;
    
    // Start is called before the first frame update
    void Start()
    {
        start_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Time since game start
        play_time = Time.time - start_time;

        time_text.text = System.String.Format("{0:0.0}",
                        (play_time).ToString("F2"));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newRoadScript : MonoBehaviour
{
    public float startpos_x;
    public float startpos_y;
    public float startpos_z;
    public float roadspeed;
    public float pos;
    public float other_road_pos;
    public float scale;
    public float other_road_scale;
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale.z;
        other_road_scale = GameObject.Find("road1a").transform.localScale.z;
        startpos_x = 0;
        startpos_y = 0;
        startpos_z = 139;
        roadspeed = -50;
        transform.position = new Vector3(startpos_x, startpos_y, startpos_z);
        velocity = new Vector3(0, 0, -50);
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position.z;
        if (pos < -(scale / 2) - 4)
        {
            other_road_pos = GameObject.Find("road 2").transform.position.z;
            transform.position = new Vector3(0, 0, (other_road_pos + (other_road_scale / 2) + (scale / 2)));
        }
        transform.position += velocity * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class road_1_movement : MonoBehaviour
{
    // public Rigidbody rb;
    public float startpos_x;
    public float startpos_y;
    public float startpos_z;
    public float roadspeed;
    public float pos;
    public float other_road_pos;
    public float scale;
    public float other_road_scale;
    public Vector3 velocity;

    private GameObject player_obj = null;
    private float player_pos;

    // Start is called before the first frame update
    void Start()
    {
        if (player_obj == null)
            player_obj = GameObject.FindGameObjectWithTag("Player");

        // rb.useGravity = false;
        scale = transform.localScale.z;
        other_road_scale = GameObject.Find("road 2").transform.localScale.z;
        startpos_x = 0;
        startpos_y = 0;
        startpos_z = ((scale/2)-10);
        roadspeed = -50;
        transform.position = new Vector3(startpos_x, startpos_y, startpos_z);
        velocity = new Vector3(0, 0, -50);
        // rb.velocity = new Vector3(0, 0, roadspeed);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player_pos = player_obj.transform.position.z;
        pos = transform.position.z;
        if (pos < player_pos -(scale/2) - 15)
        {
            Debug.Log("pos: " + pos.ToString() + ", condition: " +
            (player_pos -(scale/2) - 15).ToString() + ", player: "
              + player_pos.ToString() + ", scale: " + scale.ToString());
            other_road_pos = GameObject.Find("road 2").transform.position.z;
            transform.position = new Vector3(0, 0, (other_road_pos + (other_road_scale / 2) + (scale / 2)));
        }
        transform.position += velocity * Time.deltaTime;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Rigidbody player;
    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 4.57f, -10);
        transform.rotation = Quaternion.identity;
        pos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 4.57f, player.position.z - 10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anchorMoverScript : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void Zmove(float crashZVelocity)
    {
        rb.velocity = new Vector3(0, 0, crashZVelocity);
    }
}

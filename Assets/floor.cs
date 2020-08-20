using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor : MonoBehaviour
{

    // the "floor" is just a static flat cube that does not get rendered.  it keeps the blocks and player from falling.


    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

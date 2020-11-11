using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunScript : MonoBehaviour
{
    private Vector3 offsetFromPlayer;
    public Transform cameraTransform;
    private Vector3 sunPosition;
    // Start is called before the first frame update
    void Start()
    {
        offsetFromPlayer = new Vector3(0, 39.95f, 593.6f);
        transform.position = offsetFromPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraTransform.position + offsetFromPlayer;
    }
}

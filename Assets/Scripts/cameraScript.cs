using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Rigidbody player;
    public Vector3 pos;
    public float cameraDistance;        // camera distance, camera height, fov, & angle are now public variables so that the view can be changed for debugging purposes.  
    public float cameraHeight;          // for a mobile setup, we want the camera closer so we can see the obstacles and player vehicle  more clearly on a small screen.
    public float fov;
    public float cameraAngle;
    private Quaternion cameraRotation;

    public bool smoothDampOn;
    public float smoothTime;
    private float velocityRef;
    private float dampPosition;
    public float cameraMobility;

    // Start is called before the first frame update
    void Start()
    {
        smoothDampOn = true;
        cameraDistance = 12.92f;
        cameraHeight = 4.53f;
        fov = 45.55f;
        cameraAngle = 1.01f;
        transform.position = new Vector3(0, cameraHeight, -cameraDistance);
        cameraRotation = Quaternion.Euler(cameraAngle, 0, 0);
        transform.rotation = cameraRotation;
        Camera.main.fieldOfView = fov;
        //transform.rotation = Quaternion.identity;
        pos = player.transform.position;

        velocityRef = 0;
        smoothTime = 0.25f;

        cameraMobility = 0.5f;

    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (smoothDampOn == true)
        {
            dampPosition = Mathf.SmoothDamp(Camera.main.transform.position.x, (player.position.x) * cameraMobility, ref velocityRef, smoothTime);
        }
        else
        {
            dampPosition = 0;
        }

        transform.position = new Vector3(dampPosition, cameraHeight, player.position.z - cameraDistance);
        cameraRotation = Quaternion.Euler(cameraAngle, 0, 0);
        transform.rotation = cameraRotation;
        Camera.main.fieldOfView = fov;

    }
}

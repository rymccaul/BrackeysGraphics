using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_mirror_script : MonoBehaviour
{
    private GameObject reflected_npc;
    Quaternion relativeRotation;
    
    private bool isOnRoad = true;
    private bool meshRendererCurrentValue = true;

    // Start is called before the first frame update
    void Start()
    {
        // Create copy of npc and disable box collider for reflection
        reflected_npc = Instantiate(gameObject);
        reflected_npc.GetComponent<BoxCollider>().enabled = false;

        // remove reflection's movement script, and this script to avoid
        // recursive behaviour, reflection of reflections. Remove sensor.
        Destroy(reflected_npc.GetComponent("obstacle_movement"));
        Destroy(reflected_npc.GetComponent("npc_mirror_script"));
        Destroy(reflected_npc.GetComponent<Transform>().GetChild(0).gameObject);

        // Set initial relative rotation of reflection to 180 deg flip
        reflected_npc.transform.rotation = transform.rotation *
                                               Quaternion.Euler(0f, 0f, 180f);

       // Get initial relative rotation between reflection and original
       relativeRotation = Quaternion.Inverse(reflected_npc.transform.rotation) *
                                                            transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Mirror point will be point on the road under original
        Vector3 mirror_point = new Vector3(transform.position.x, 0.4f,
                                                        transform.position.z);

        // Set position to offset away from mirror point
        reflected_npc.transform.position = Vector3.LerpUnclamped(
                                        transform.position, mirror_point, 2f);

        // Set rotation to mimic original
        reflected_npc.transform.rotation = transform.rotation * relativeRotation;

        if (reflected_npc.transform.position.x > 7.5 || reflected_npc.transform.position.x < -7.5)
        {
            isOnRoad = false;
        } else {
            isOnRoad = true;
        }

        setMeshRenderer();
    }

    void setMeshRenderer()
    {
        if (meshRendererCurrentValue != isOnRoad)
        {
            reflected_npc.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = isOnRoad;
            meshRendererCurrentValue = isOnRoad;
        }
    }
}

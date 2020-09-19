using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blast_reflection : MonoBehaviour
{
    public GameObject blast_trigger;

    private GameObject reflected_trigger;

    // Start is called before the first frame update
    void Start()
    {
        reflected_trigger = Instantiate(blast_trigger);
        reflected_trigger.GetComponent<BoxCollider>().enabled = false;

        Destroy(reflected_trigger.GetComponent("blastScript"));
    }

    // Update is called once per frame
    void Update()
    {
        // Mirror point will be point on the road under trigger
        Vector3 mirror_point = new Vector3(blast_trigger.transform.position.x, 0.4f,
                                                blast_trigger.transform.position.z);

        // Set position to offset away from mirror point
        reflected_trigger.transform.position = Vector3.LerpUnclamped(
                                    blast_trigger.transform.position, mirror_point, 2f);
    }
}

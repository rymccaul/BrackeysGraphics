using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_mirror_script : MonoBehaviour
{
    public GameObject player;

    private GameObject reflected_player;
    Quaternion relativeRotation;

    // Start is called before the first frame update
    void Start()
    {
         // Create copy of player and disable box collider for reflection
         reflected_player = Instantiate(player);
         reflected_player.GetComponent<BoxCollider>().enabled = false;

         //remove back and front hinge, and player movement script from relfection
         Destroy(reflected_player.GetComponent("Player_movement"));
         Destroy(reflected_player.GetComponent<Transform>().GetChild(0).gameObject);
         Destroy(reflected_player.GetComponent<Transform>().GetChild(1).gameObject);

         // Set initial relative rotation of reflection to 180 deg flip
         reflected_player.transform.rotation = player.transform.rotation * Quaternion.Euler(0f, 0f, 180f);

         // Get initial relative rotation between reflection and original
         relativeRotation = Quaternion.Inverse(reflected_player.transform.rotation) * player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Mirror point will be point on the road under player
        Vector3 mirror_point = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        // Set position to offset away from mirror point
        reflected_player.transform.position = Vector3.LerpUnclamped(player.transform.position, mirror_point, 2f);

        // Set rotation to mimic original
        reflected_player.transform.rotation = player.transform.rotation * relativeRotation;
    }
}

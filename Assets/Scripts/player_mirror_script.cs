using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class player_mirror_script : MonoBehaviour
{
    public GameObject player;

    private GameObject reflected_player;
    Quaternion relativeRotation;

    private bool isOnRoad = true;
    private bool meshRendererCurrentValue = true;

    private TextMeshProUGUI gameover_text = null;

    // Start is called before the first frame update
    void Start()
    {
         gameover_text = GameObject.Find("UI_game_over").GetComponent<TextMeshProUGUI>();

         // Create copy of player and disable box collider for reflection
         reflected_player = Instantiate(player);
         reflected_player.GetComponent<BoxCollider>().enabled = false;

         // remove back and front hinge, and player movement script from relfection
         Destroy(reflected_player.GetComponent("Player_movement"));
         Destroy(reflected_player.GetComponent<Transform>().GetChild(0).gameObject);
         Destroy(reflected_player.GetComponent<Transform>().GetChild(1).gameObject);

         // Set initial relative rotation of reflection to 180 deg flip
         reflected_player.transform.rotation = player.transform.rotation *
                                                Quaternion.Euler(0f, 0f, 180f);

         // Get initial relative rotation between reflection and original
         relativeRotation = Quaternion.Inverse(reflected_player.transform.rotation) *
                                                    player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Mirror point will be point on the road under player
        Vector3 mirror_point = new Vector3(player.transform.position.x, 0.4f,
                                                player.transform.position.z);

        // Set position to offset away from mirror point
        reflected_player.transform.position = Vector3.LerpUnclamped(
                                    player.transform.position, mirror_point, 2f);

        // Set rotation to mimic original
        reflected_player.transform.rotation = player.transform.rotation * relativeRotation;

        if (reflected_player.transform.position.x > 7.5 || reflected_player.transform.position.x < -7.5)
        {
            isOnRoad = false;
            gameover_text.enabled = true;
            Time.timeScale = 0f;
        }
        else
        {
            isOnRoad = true;
        }

        setMeshRenderer();
    }

    // Using function and extra variable simply to avoid setting the meshRenderer on every frame unless
    // there has actually been a change to the isOnRoad variable.
    void setMeshRenderer()
    {
        if (meshRendererCurrentValue != isOnRoad)
        {
            reflected_player.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = isOnRoad;
            meshRendererCurrentValue = isOnRoad;
        }
    }
}

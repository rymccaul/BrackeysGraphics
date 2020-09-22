using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup_sphere_script : MonoBehaviour
{
    public GameObject player;
    public GameObject jump_sphere;
    public Material ready_mat;
    public Material not_ready_mat;

    private MeshRenderer meshRenderer;
    private Material SphereMaterial;

    // Start is called before the first frame update
    void Start()
    {
        SphereMaterial = Resources.Load<Material>("SphereMaterial");
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        jump_sphere.transform.position =
                            new Vector3(0, 6, player.transform.position.z - 3);
    }

    public void setNotReady(){
        meshRenderer.sharedMaterial = not_ready_mat;
    }

    public void setReady(){
        meshRenderer.sharedMaterial = ready_mat;
    }
}

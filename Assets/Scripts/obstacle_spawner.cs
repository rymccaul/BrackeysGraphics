using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle_spawner : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject car_UV;

    public Material car_material;

    public int traffic_count;

    public Renderer r;

    public Color[] colors;

    private const string base_color_ref = "Color_F8306B56";
    private const string trim_color_ref = "Color_7E447BC2";

    // Start is called before the first frame update
    void Start()
    {
        car_material.SetColor("base-color", Color.red);
        // Debug.Log(car_material.GetColor("base-color"));

        Component[] mesh_renderers = obstacle.GetComponentsInChildren(typeof(MeshRenderer));

        foreach(MeshRenderer mrenderer in mesh_renderers)
        {
            if(mrenderer.tag == "carUV")
            {
                mrenderer.material = car_material;
                Debug.Log(mrenderer.ToString());
                //Destroy();
                //obstacle.AddComponent(mrenderer);
            }
        }



        //MeshRenderer car_material_meshr = obstacle.GetComponentInChildren(typeof(MeshRenderer), true) as MeshRenderer;

        for(int i = 0; i < traffic_count; i++)
        {
            //car_material_meshr.material = car_material;
            // Debug.Log(car_material);
            // Debug.Log(car_material_meshr);
            //car_UV.transform.SetParent(obstacle.transform);

            Instantiate(obstacle, new Vector3(0, 0, -(i + 100)), Quaternion.identity);
        }
    }
}

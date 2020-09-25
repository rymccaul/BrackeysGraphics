using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup_sphere_script : MonoBehaviour
{

    public Material ready_mat;
    public Material not_ready_mat;

    public GameObject player;
    public GameObject jump_sphere;
    public GameObject boost_sphere;
    public GameObject blast_sphere;

    private MeshRenderer jump_meshr;
    private MeshRenderer boost_meshr;
    private MeshRenderer blast_meshr;

    //private Material SphereMaterial;
    private Player_movement player_movement_script;

    private float boost_recharge;
    private float jump_recharge;
    private float blast_recharge;

    // Start is called before the first frame update
    void Start()
    {
        //SphereMaterial = Resources.Load<Material>("SphereMaterial");
        jump_meshr = jump_sphere.GetComponent<MeshRenderer>();
        boost_meshr = boost_sphere.GetComponent<MeshRenderer>();
        blast_meshr = blast_sphere.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        player_movement_script = player.GetComponent<Player_movement>();

        boost_recharge = player_movement_script.boostRecharge;
        jump_recharge = player_movement_script.jumpRecharge;
        blast_recharge = player_movement_script.blastRecharge;

        if(jump_recharge <= 0) setReady(jump_meshr); else setNotReady(jump_meshr);
        if(boost_recharge <= 0) setReady(boost_meshr); else setNotReady(boost_meshr);
        if(blast_recharge <= 0) setReady(blast_meshr); else setNotReady(blast_meshr);

        jump_sphere.transform.position =
                            new Vector3(0, 6, player.transform.position.z - 3);
        boost_sphere.transform.position =
                            new Vector3(0.5f, 6, player.transform.position.z - 3);
        blast_sphere.transform.position =
                            new Vector3(-0.5f, 6, player.transform.position.z - 3);
    }

    public void setNotReady(MeshRenderer sphere){
        sphere.sharedMaterial = not_ready_mat;
    }

    public void setReady(MeshRenderer sphere){
        sphere.sharedMaterial = ready_mat;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorOpenner : MonoBehaviour
{
    public Collider trigger_space;
    public Animator animator;

    private float current_time = 0f;
    public float total_time = 5f;

    private bool countdown_started = false;
    private bool is_hacked = false;

    public TextMeshProUGUI text_pourcent_before_opening;

    public GameObject light_cube;
    private Renderer light_cube_renderer;

    private Dictionary<string, Color> color_list = new Dictionary<string, Color>();

    public bool has_disc = false;


    // Start is called before the first frame update
    void Start()
    {
        light_cube_renderer = light_cube.GetComponent<Renderer>();
        color_list["green"] = new Color(0f,1f,0f);
        color_list["yellow"] = new Color(1f, 1f, 0f);
        color_list["red"] = new Color(1f, 0f, 0f);
        light_cube_renderer.material.SetColor("_Color", color_list["red"]);

        //disc : 
        GameEvents.current.onDiscPickup += PickupDisc;
    }
    void PickupDisc()
    {
        if (has_disc)
        {
            has_disc = false;
        }
        else
        {
            has_disc = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown_started)
        {
            current_time += Time.deltaTime;
            if (current_time > total_time)
            {
                // Door opens
                open_door();
                countdown_started = false;
                light_cube_renderer.material.SetColor("_Color", color_list["green"]);
            }
            int pourcent = (int)(current_time / total_time * 100);
            text_pourcent_before_opening.text = pourcent.ToString() + "%\nHACKED";

        }
    }

    void open_door()
    {
        is_hacked = true;
        animator.SetBool("Door is open", true);
    }

    private void OnTriggerExit(Collider other)
    {
        GameEvents.current.PlayerInZone();
    }
    private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.PlayerInZone();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        if (countdown_started || is_hacked) return;
        //GameEvents.current.PlayerInZone();
        if (Input.GetKey(KeyCode.E) && has_disc)
        {
            GameEvents.current.DiscPickup();
            GameEvents.current.PlayerInteractDoor();
            countdown_started = true;
            current_time = 0;
            light_cube_renderer.material.SetColor("_Color", color_list["yellow"]);
        }
        
    }
}

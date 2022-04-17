using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    private bool is_in_zone = false;
    private bool done_interaction = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(false);
        GameEvents.current.playerInInteractZone += PlayerInZone;
        GameEvents.current.playerInteractDoor += PlayerInteracted;
    }

    void PlayerInZone()
    {
        if (is_in_zone || done_interaction)
        {
            is_in_zone = false;
        }
        else
        {
            is_in_zone = true;
        }
        transform.gameObject.SetActive(is_in_zone);

    }

    void PlayerInteracted()
    {
        if (done_interaction)
        {
            done_interaction = false;
        }
        else
        {
            done_interaction = true;
        }
        PlayerInZone();
    }

}

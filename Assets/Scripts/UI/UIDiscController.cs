using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDiscController : MonoBehaviour
{
    bool have_disc = false;

    private void Start()
    {
        GameEvents.current.onDiscPickup += PickupDisc;
        transform.gameObject.SetActive(false);
    }

    void Update()
    {
        // nothing for the moment 
    }

    void PickupDisc()
    {
        if (have_disc)
        {
            have_disc = false;
        }
        else
        {
            have_disc = true;
        }
        transform.gameObject.SetActive(have_disc);
    }
}

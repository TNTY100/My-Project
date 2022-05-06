using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float health = 100;

    public bool has_disc = false;
    private void Start()
    {
        GameEvents.current.onDiscPickup += PickupDisc;
    }
    void Update()
    {
        // nothing for the moment 
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
}

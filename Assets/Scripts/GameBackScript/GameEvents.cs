using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action onDiscPickup;

    public void DiscPickup()
    {
        if (onDiscPickup != null)
        {
            onDiscPickup();
        }
    }

    public event Action playerInInteractZone;

    public void PlayerInZone()
    {
        if (playerInInteractZone != null)
        {
            playerInInteractZone();
        }
    }

    public event Action playerInteractDoor;
    public void PlayerInteractDoor()
    {
        if (playerInteractDoor != null)
        {
            playerInteractDoor();
        }
    }
}

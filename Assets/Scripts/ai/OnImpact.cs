using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnImpact : MonoBehaviour
{

    // public GameObject impactEffect;
    private GameObject player;

    public float damage = 25f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ContactPoint contact = collision.contacts[0];
        // Instantiate(impactEffect, contact.point, Quaternion.LookRotation(contact.normal));

        if (collision.gameObject.tag == "Player")
        {
            
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            playerScript.health -= damage;
        }

        Destroy(gameObject);
    }
}

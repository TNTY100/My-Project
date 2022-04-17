using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisquetteScript : MonoBehaviour
{
    public Collider collider;
    // public GameObject self;
    private int spin_speed_y = 45;

    
    // Start is called before the first frame update
    void Start()
    {
        //self = this;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, spin_speed_y, 0) * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        GameEvents.current.DiscPickup();
        transform.gameObject.SetActive(false);
    }

}

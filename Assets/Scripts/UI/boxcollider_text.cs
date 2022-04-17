using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxcollider_text : MonoBehaviour
{
    public Collider collider;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Toucher" + collider.isTrigger);
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(false);
        transform.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

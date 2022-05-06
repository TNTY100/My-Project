using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBehaviorShoot : MonoBehaviour
{
    public GameObject Bullet;

    //shot timing 
    private float timerShots;
    public float timeBtwShots = 0.25f;

    public float fireRadius = 25f;
    public float Force = 2000f;

    private Vector3 playerPos;
    private GameObject player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        float distance = Vector3.Distance(playerPos, transform.position);
        if (distance <= fireRadius)
        {
            Debug.Log("SHOOT2");
            AI_FireBullet();
        }
    }

    void AI_FireBullet()
    {
        RaycastHit hitPlayer;
        Ray playerPos = new Ray(transform.position, transform.forward);
        Debug.Log("Shoot1");
        if (Physics.SphereCast(playerPos, 0.25f, out hitPlayer, fireRadius))
        {
            Debug.Log("Shoot2");
            if (timerShots <=0 && hitPlayer.transform.tag == "Player")
            {
                Debug.Log("Shoot3");
                GameObject BulletHolder;
                BulletHolder = Instantiate(Bullet, transform.position, transform.rotation) as GameObject;
                BulletHolder.transform.Rotate(Vector3.left * 90);

                Rigidbody Temp_RigidBody;
                Temp_RigidBody = BulletHolder.GetComponent<Rigidbody>();

                Temp_RigidBody.AddForce(transform.forward * Force);


                Destroy(BulletHolder, 2.0f);

                timerShots = timeBtwShots;
            }
            else
            {
                timerShots -= Time.deltaTime;

            }
        }
    }
}

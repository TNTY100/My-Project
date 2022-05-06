using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;


//À ajouter bail du scriptable object et tout!!!

public class GunController : MonoBehaviour
{
    public GunData gun_data;
    private int current_ammo;
    private float next_time_to_fire = 0;
    private bool is_reloading;


    public Camera fps_cam;

    public ParticleSystem muzzle_flash;
    public GameObject impact_effect;
    
    public Animator animator;

    private AnimationClip[] clips;
    private float shouting_animation_time;

    public TextMeshProUGUI ammo_counter;


    void Start()
    {
        current_ammo = gun_data.max_ammo;
        clips = animator.runtimeAnimatorController.animationClips;
        shouting_animation_time = clips[0].length;

    }

    private void OnEnable()
    {
        is_reloading = false;
        animator.SetBool("reloading", false);
    }
    void Update()
    {
        int ammo_before_refresh = current_ammo;

        if (is_reloading)
        {
            // animator.SetBool("shooting", false); 
            return;
        }
        if (current_ammo <= 0 || Input.GetButton("Reloading") && current_ammo != gun_data.max_ammo)
        {
            StartCoroutine(Reload());
            return;
        }

        // Si le gun est automatic 
        if (!gun_data.is_semi_auto && Input.GetButton("Fire1") && Time.time >= next_time_to_fire)
        {
            // Control de rate of fire.
            next_time_to_fire = Time.time + 1f / gun_data.fire_rate;
            animator.SetBool("shooting", true);
            Shoot();
        }
        // Si le gun est semi-automatic
        if (gun_data.is_semi_auto && Input.GetButtonDown("Fire1") && Time.time >= next_time_to_fire)
        {
            // Control de rate of fire.
            next_time_to_fire = Time.time + shouting_animation_time;
            animator.SetBool("shooting", true);
            Shoot();
            
        }

        
        if (Time.time >= next_time_to_fire)
        {
            animator.SetBool("shooting", false);
        }

        if (ammo_before_refresh != current_ammo)
        {
            ammo_counter.text = current_ammo.ToString() + "/" + gun_data.max_ammo.ToString();
        }
    }


    IEnumerator Reload()
    {
        animator.SetBool("shooting", false);
        is_reloading = true;
        Debug.Log("Reloading...");


        animator.SetBool("reloading", true);

        yield return new WaitForSeconds(gun_data.reload_time);

        animator.SetBool("reloading", false);
        current_ammo = gun_data.max_ammo;
        is_reloading = false;
        ammo_counter.text = current_ammo.ToString() + "/" + gun_data.max_ammo.ToString();
    }




    void Shoot()
    {

        muzzle_flash.Play();

        current_ammo--;

        RaycastHit hit;
        if (Physics.Raycast(fps_cam.transform.position, fps_cam.transform.forward, out hit, gun_data.range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(gun_data.damage);
            }

            // Add force to a hit rigidbody. 
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * gun_data.impact_force);
            }

            GameObject impact_go = Instantiate(impact_effect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact_go, 1f);
        }
        
    }
}
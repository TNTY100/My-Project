using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//nouvel essai

[CreateAssetMenu(fileName = "Gun Data", menuName = "ScriptableObjects/GunData", order = 1)]

public class GunData : ScriptableObject
{
    public float damage, range, fire_rate, impact_force;  // timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int max_ammo;
    public float reload_time;
    public bool is_semi_auto;
    
    
    
    // public int magazineSize, bulletsPerTap;
    // public bool allowButtonHold;
    // int bulletsLeft, bulletsShot;
    // public GameObject gunModel;
    // public GameObject shotLight;

}

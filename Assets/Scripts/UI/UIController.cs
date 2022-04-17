using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{
    public Text health_text;
    public Image health_bar;

    // risque d'être modifié 
    float health, max_health = 100;

    // Start is called before the first frame update
    void Start()
    {
        health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        health_text.text = "Health :" + health + "%";
        if (health > max_health) health = max_health;

        HealthBarFiller();

    }

    void HealthBarFiller()
    {
        health_bar.fillAmount = health / max_health;
    }

    public void Damage(float damagePoints)
    {
        if (health > 0)
        {
            health -= damagePoints;
        }
    }

    public void Heal(float healingPoints)
    {
        if (health < max_health)
        {
            health += healingPoints;
        }

    }
}

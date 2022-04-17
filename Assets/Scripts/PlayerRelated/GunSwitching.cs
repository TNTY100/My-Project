using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunSwitching : MonoBehaviour
{

    public int selected_weapon = 0;


    private bool is_switching = false;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previous_selected_weapon = selected_weapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selected_weapon >= transform.childCount - 1)
                selected_weapon = 0;
            else
                selected_weapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selected_weapon <= 0)
                selected_weapon = transform.childCount - 1;
            else
                selected_weapon--;
        }

        GetKeyPressed();
            
        if (previous_selected_weapon != selected_weapon)
        {
            Debug.Log("Switching...1");
            StartCoroutine(Reload());
            
        }
        
    }

    void GetKeyPressed()
    {
        int child_amount = transform.childCount;
        for (int i = 0; i <= child_amount - 1; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selected_weapon = i;
                // Debug.Log("Weapon slot:" + selected_weapon);
            }
        }
        

    }

    IEnumerator Reload()
    {
        Debug.Log("Switching...2");
        is_switching = true;
        animator.SetBool("Switching", true);
        yield return new WaitForSeconds(0.5f);
        SelectWeapon();
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Switching", false);
        is_switching = false;
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selected_weapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
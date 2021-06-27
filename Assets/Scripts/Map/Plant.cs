using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

    public GameObject bramble;
    Animator brambleAnim;

        
    Traps trapAttached = null;
    Animator anim;

    GameObject brambleObj;
    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();

        if (trapAttached != null)
        {
            if (trapAttached.IsFrozen())
            {
                Destroy(gameObject);
                ManagerGame.Instance.DestroyPlant(this);
            }
                
            else
                FrozeTrap(true);
        }
            
    }


    public void AssignTrap(Traps trap)
    {
        trapAttached = trap;
    }

    void FrozeTrap(bool Froze)
    {
        trapAttached.Froze(Froze);
        if (Froze)
        {
            brambleObj = Instantiate(bramble, trapAttached.transform.position, Quaternion.identity);
            brambleAnim = brambleObj.GetComponent<Animator>();
        }
    }

    public void Die()
    {
        FrozeTrap(false);
        anim.SetBool("Die", true);
        brambleAnim.SetBool("Die", true);
        StartCoroutine(DestroyObject());

    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(brambleObj);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}

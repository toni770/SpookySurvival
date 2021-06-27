using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attacks twice before reload
public class TwiceTrap : Traps
{
    [Header("TwiceTrap Config")]
    public int numOfAttacks = 3;

    int attackCount = 0;

    protected override void Activate() //Trap attacks
    {
        anim.SetInteger("State", 2);

        StartCoroutine(ActivateZone());
        
        attackCount++;
        //if (anim != null) anim.SetInteger("State", 1);
        if (attackCount >= numOfAttacks)
        {
            anim.SetBool("Twice", false);
            attackCount = 0;
            StartReload();
        }
        else
        {
            anim.SetBool("Twice", true);
        }
    }

    protected override IEnumerator ActivateZone()
    {
        yield return new WaitForSeconds(timeAnimBefore);
        damageZone.SetActive(true);
        StartCoroutine(DisableDamageZone());
        particle.Play();
    }
}

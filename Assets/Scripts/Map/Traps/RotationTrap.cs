using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Trap that rotates while waiting to ativate
public class RotationTrap : Traps
{
    [Header("RotationTrap Config")]
    public float rotSpeed = 5;
    public float timeAttack = 3;

    float attackCount = 0;
    bool attacking = false;

    protected override void Update()
    {
        base.Update();
        if(actualState != States.Reloading) transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }

    protected override void Activate() //Trap attacks
    {
        attacking = false;
        damageZone.SetActive(false);
        StartReload();
    }

    protected override void Activating()
    {
        if(!attacking)
        {
            activationCount += Time.deltaTime;
            if (activationCount >= timeToActive)
            {
                activationCount = 0;
                attacking = true;
                damageZone.SetActive(true);
                anim.SetInteger("State", 2);
            }
        }
        else
        {
            attackCount += Time.deltaTime;
            if (attackCount >= timeAttack)
            {
                attackCount = 0;
                Activate();
            }
        }

    }


}

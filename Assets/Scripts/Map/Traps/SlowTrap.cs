using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Slow down all units that enters in his area
public class SlowTrap : Traps
{
    [Header("Slow Config")]
    public float slowDuration = 5;

    float slowCount = 0;

    protected override void Start()
    {
        base.Start();
        actualState = States.Reloading;
    }

    protected override void Reloaded() //Trap Reloaded
    {
        actualState = States.Activating;
        ChangeAnim();
    }

    protected override void Waiting()
    {
        slowCount += Time.deltaTime;
        if(slowCount>slowDuration)
        {
            slowCount = 0;
            damageZone.SetActive(false);
            actualState = States.Reloading;
            ChangeAnim();
        }
    }

    protected override void Activate() //Trap attacks
    {
        damageZone.SetActive(true);
        actualState = States.Waiting;
    }   
}

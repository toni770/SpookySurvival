using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Continuously shoots bullets
public class ShootTrap : Traps
{
    [Header("Shoot Config")]
    public GameObject bullet;

    GameObject bl;

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

    protected override void Activate() //Trap attacks
    {
        StartReload();
        bl = Instantiate(bullet,transform.position,Quaternion.identity);
        bl.GetComponent<Bullet>().Inicialize(transform.forward);
    }
}

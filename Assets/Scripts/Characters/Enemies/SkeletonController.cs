using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When dies, keep reviving while player doesn't collide with it.
public class SkeletonController : EnemyController
{
	///////////PUBLIC VARS/////////
    [Header("Skeleton Properties")]
    public float timeToRevive = 5;

	///////////PRIVATE VARS/////////
    bool reviving = false;
    float countReviving = 0;


    protected override void Update()
    {
        base.Update();
        if(reviving)
        {
            countReviving += Time.deltaTime;
            if (countReviving >= timeToRevive)
            {
                countReviving = 0;
                reviving = false;
                Revive();

            }
        }
    }
    public override void Death()//Called by: Manager(when DeathPowerUp getted)
    {
        anim.SetBool("Dead", true);
        nav.isStopped = true;
        reviving = true;
    }

    public void PermaDeath()//Called by: Player(when Player collides)
    {
        reviving = false;
        base.Death();
    }

    public bool IsReviving()//Called by: Player(when Player collides to check)
    {
        return reviving;
    }
	
    void Revive()
    {
        anim.SetBool("Dead",false);
        nav.isStopped = false;
        reviving = false;
    }
}


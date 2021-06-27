using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//It makes invencible for a while.
public class GhostController : EnemyController
{
	///////////PUBLIC VARS/////////
	[Header("Ghost Config")]
	public float timeToInvecible = 5;
    public float invencibleDuration = 3;
	
	///////////PRIVATE VARS/////////
    bool invencible;
    float invCount;

    ///////////FUNCTIONS/////////
    void Start()
    {
        invencible = false;
        invCount = 0;
    }

    protected override void Update()
    {
        base.Update();
        invCount += Time.deltaTime;
        if (!invencible)
        {
            if(invCount>timeToInvecible)
            {
                invCount = 0;
                invencible = true;
                getInvisible(invencible);
            }
        }
        else
        {
            if (invCount > invencibleDuration)
            {
                invCount = 0;
                invencible = false;
                getInvisible(invencible);
            }
        }
    }

    protected override void GetDamage(int damage)
    {
        if(!invencible)
            base.GetDamage(damage);
    }

    void getInvisible(bool inv)
    {
        anim.SetBool("Transparent", inv);
    }
}

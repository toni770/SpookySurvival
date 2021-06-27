using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Have an armor that makes him inmune to Traps, but it makes him slower.
public class WarriorController : PlayerController
{
	///////////PUBLIC VARS/////////
	[Header("WarriorConfig")]
    public float speedInc = 3f;
    public float timeRepairArmor = 60;
    public GameObject armor;
    ///////////PRIVATE VARS/////////
    bool hasArmor;
    bool armorAvaliable = true;

    float initialPowerTime;

    ///////////FUNCTIONS/////////
    private void Start()
    {
        anim.SetBool("Shield", true);
        initialPowerTime = habilityCD;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
	
    protected override void Inicialize()
    {
        base.Inicialize();
        hasArmor = true;
    }

    protected override void ReduceLife(int damage, string origin)
    {
        if(hasArmor)
        {
            if (origin != "Daño")
            {
                HitWithArmor();
            }
        }
        else
        {
            base.ReduceLife(damage, origin);
        }
    }
	
	void LoseArmor()
    {
        if(armorAvaliable)
        {
            anim.SetBool("Shield",false);
            armor.SetActive(false);
            hasArmor = false;
            ChangeVel(speedInc);
        }

    }

    void RestoreArmor()
    {
        if (armorAvaliable)
        {
            anim.SetBool("Shield", true);
            armor.SetActive(true);
            hasArmor = true;
            ChangeVel(-speedInc);
        }
    }

    protected override void DoPower()
    {
        if (hasArmor)
            LoseArmor();
        else
            RestoreArmor();
    }

    protected override void EnablePower()
    {
        if (!armorAvaliable)
        {
            armorAvaliable = true;
            habilityCD = initialPowerTime;
        }
        base.EnablePower();
    }

    void HitWithArmor()
    {
        LoseArmor();
        habilityCD = timeRepairArmor;
        GetInmunity();
        armorAvaliable = false;
        DisablePower();
    }

}

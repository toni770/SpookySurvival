using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can activate god mode to kill enemies, but will get tired and need to charge on a charging station
public class GollemController : PlayerController
{
    [Header("GollemConfig")]
    public float energyIncrement = 1;
    public float energyDecrement = 10;
    public float reloadIncrement = 5;
    public float speedTiredDecrement = 4;
    public float speedGodIncrement = 4;
    public float attackSpeed = 1;
    public ParticleSystem effect;

    public GameObject damageZone;

    enum State { Tired, Recharge, Normal, God}
    State actualState = State.Normal;

    bool fullLoaded = false;

    float energy = 0;
    float attackCount = 0;

    private void Start()
    {
        energy = 50;
        ChangeEnergyUI();
        ChangeState(State.Normal);
        DisablePower();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Controller();
    }

    void Controller()
    {
        switch(actualState)
        {
            case State.Tired:
                break;
            case State.Recharge:
                ReloadEnergy();
                break;
            case State.Normal:
                IncrementEnergy();
                break;
            case State.God:
                GodMode();
                break;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(other.tag == "MagicStone" && actualState == State.Tired)
        {
            TiredToRecharge();
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.tag == "MagicStone" && actualState == State.Recharge)
        {
            RechargeToTired();
        }
    }
    protected override void DoPower()
    {
        if(fullLoaded)
            NormalToGod();
    }

    protected override void EnablePower()
    {
        if (fullLoaded)
            base.EnablePower();
    }

    void GodMode()
    {
        DecrementEnergy();
        if(energy > 5)
        {
            attackCount += Time.deltaTime;
            if (attackCount > attackSpeed)
            {
                attackCount = 0;
                damageZone.SetActive(true);
                StartCoroutine(disableDamage());
            }
        }

    }

    IEnumerator disableDamage()
    {
        yield return new WaitForSeconds(0.1f);
        damageZone.SetActive(false);
    }
    void DecrementEnergy()
    {
        energy -= Time.deltaTime * energyDecrement;
        if(energy <= 0)
        {
            energy = 0;
            GodToTired();
        }
        ChangeEnergyUI();
    }
    void IncrementEnergy()
    {
        if(!fullLoaded)
        {
            energy += Time.deltaTime * energyIncrement;
            if(energy>= 100)
            {
                energy = 100;
                fullLoaded = true;
                EnablePower();
            }
        }
        ChangeEnergyUI();
    }

    void ReloadEnergy()
    {
        energy += Time.deltaTime * reloadIncrement;
        if (energy >= 50)
        {
            energy = 50;
            RechargeToNormal();
        }
        ChangeEnergyUI();
    }

    void ChangeEnergyUI()
    {
        ManagerGame.Instance.ChangeEnergy(energy);
    }
    void ChangeState(State st)
    {
        actualState = st;
        SetAnim();
    }

    //STATES CHANGE
    public void TiredToRecharge()
    {
        ChangeState(State.Recharge);
        effect.Play();
    }
    public void RechargeToTired()
    {
        ChangeState(State.Tired);
        effect.Stop();
    }
    public void RechargeToNormal()
    {
        ChangeState(State.Normal);
        ChangeVel(speedTiredDecrement);

        effect.Stop();

        ManagerGame.Instance.EnableMagicStones(false);
    }
    public void NormalToGod()
    {
        fullLoaded = false;
        GetInmunity();
        ChangeVel(speedGodIncrement);
        attackCount = 0;
        ChangeState(State.God);
    }
    public void GodToTired()
    {
        StopInmunity();
        ChangeVel(-speedGodIncrement);
        ChangeVel(-speedTiredDecrement);
        ChangeState(State.Tired);

        ManagerGame.Instance.EnableMagicStones(true);
    }
    void SetAnim()
    {
        if (actualState == State.Tired)
        {
            anim.SetBool("Tired", true);
            anim.SetBool("Attack", false);
        }
        else if (actualState == State.Normal)
        {
            anim.SetBool("Tired", false);
        }
        else if(actualState == State.God)
        {
            anim.SetBool("Attack", true);
        }
    }


}

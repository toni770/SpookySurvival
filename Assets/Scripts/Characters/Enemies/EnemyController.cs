using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Base script for enemies
//Follows the player with NavMesh
//Has a life and dies when reach 0
//Has a coin attached which instantiate when dies
public class EnemyController : MonoBehaviour
{
	///////////PUBLIC VARS/////////
	[Header("EnemyConfig")]
	public int life = 1;
	public float secondsBeforeDestroy = 3;
    public float stunnedTime = 3;
    public GameObject Deatheffect;
    public GameObject body;

	///////////PRIVATE VARS/////////
    protected enum States { Walking, Stunned, Dying, Deleting }
    protected States actualState = States.Walking;
	
	//Components
    protected Animator anim;
    protected Collider col;
    protected NavMeshAgent nav;
    EnemyLootController loot;
	
	Transform Player;

    Traps trapHit = null;
	
	//Counts
	float countDestroying = 0;
    float countStunned = 0;
    
	////////UNITY FUNCTIONS///////////
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();

        Player = GameObject.FindGameObjectWithTag("Player").transform;

        anim = transform.GetComponentInChildren<Animator>();

        col = transform.GetComponent<Collider>();

        actualState = States.Walking;

        loot = GetComponent<EnemyLootController>();
    
    }

    protected virtual void Update()
    {
        StateManager();

    }
	
	private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Daño")
        {
            GetDamage(1);

            if (other.GetComponent<DamageZone>() != null)
            {
                trapHit = other.GetComponent<DamageZone>().GetOriginTrap();
            }
            else
                trapHit = null;
        }
    }

	////////PUBLIC FUNCTIONS///////////
    public void VelIncrement(float value) //Called by Manager(when difficult increase)
    {
        nav.speed += value;
    }
	
    public virtual void Death() //Called by Manager(when DeathPowerUp getted)
    {
        if(actualState != States.Dying)
        {
            ManagerGame.Instance.EnemyDeath();
            anim.SetTrigger("Dead");
            nav.isStopped = true;
            col.enabled = false;
            actualState = States.Dying;
        }
    }
	
	
    public void Stun(bool stunned) //Called by Manager(when StunPowerUp getted)
    {
        if (stunned)
        {
            nav.isStopped = true;
            actualState = States.Stunned;
        }
        else
        {
            nav.isStopped = false;
            actualState = States.Walking;
        }
    }

	////////PRIVATE FUNCTIONS///////////
	
    void StateManager() //Update
    {
        switch (actualState)
        {
            case States.Dying:

                countDestroying += Time.deltaTime;
                if (countDestroying >= secondsBeforeDestroy)
                {
                    AnounceDeath();
                    DestroyEnemy();
                }

                break;
            case States.Stunned:
                countStunned += Time.deltaTime;
                if (countStunned >= stunnedTime)
                {
                    countStunned = 0;
                    Stun(false);
                }
                break;
            case States.Walking:
                if (Player != null)
                    nav.SetDestination(Player.position);
                break;
        }
    }
	
    protected virtual void GetDamage(int damage)
    {
        life -= damage;
        if(life <= 0)
        {
            Death();
        }
        else
        {
            Stun(true);
        }
    }

    void AnounceDeath()
    {
        ManagerGame.Instance.SetEnemyDeath(this);
    }

    protected virtual void DestroyEnemy()
    {
        if (trapHit != null) loot.SetTrapActived(trapHit);
        loot.Loot();

        actualState = States.Deleting;
        if (Deatheffect != null)
        {
            StartCoroutine(PlayParticles());
        }
        else 
            Destroy(gameObject);
    }

    IEnumerator PlayParticles()
    {
        Deatheffect.SetActive(true);
        body.SetActive(false);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);

    }


}

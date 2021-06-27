using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main script for traps. 
    // When te player trigger the zone, the trap activates dealing damage in a specific area.
    // After that, get disabled for a while

//Animations states//
// 0-Wait
// 1-Preparing
// 2-Attack
// 3-Reload
public class Traps : MonoBehaviour
{
	///////////PUBLIC VARS/////////
	[Header("Trap Config")]
	public float activationCD = 3; //Time to attack.
    public float timeToActive = 2; //Time to restart.
    public GameObject damageZone;
    public float timeAnimBefore = 0; //Time animation plays before make damage
    public ParticleSystem particle;
    public ParticleSystem powerUpPArticle;
    bool frozen = false;
	///////////PRIVATE VARS/////////
    protected enum States { Waiting, Activating, Reloading }
    protected States actualState = States.Waiting;

    Outline outl;
	//Count vars
    protected float activationCount = 0;
    float reloadingCount = 0;

    //Provisional
    protected Renderer rend;

    protected Animator anim;
    protected int state = 0;
    bool attackInstant = false;

	///////UNITY FUNCTIONS/////
    protected virtual void Start()
    {
        //rend = GetComponent<Renderer>();
        outl = transform.GetComponentInChildren<Outline>();
        anim = transform.GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        switch (actualState)
        {
            case States.Waiting:
                Waiting(); //Waiting to Active
                break;
            case States.Activating:
                Activating(); //Preparing to attack
                break;
            case States.Reloading:
                Reloading(); //Waiting to restart
                break;
        }
    }
	
	protected virtual void OnTriggerEnter(Collider other)
	{
		if(other.transform.tag == "Player")
		{
			Activation();
		}
	}

	///////PUBLIC FUNCTIONS/////
	public void changeVel(float vel) //Called by: PowerUpManager(on get TrapVel power up)
    {
        if (powerUpPArticle != null)
        {
            if (powerUpPArticle.isPlaying) powerUpPArticle.Stop();
            else powerUpPArticle.Play();
        }

        activationCD = activationCD * vel;
    }

    public void Froze(bool froze) //Called by: Plant(on appear in)
    {
        frozen = froze;
    }

    public bool IsFrozen() //Called by: EnemyLoot(on spawn plant)
    {
        return frozen;
    }
	
	///////PRIVATE FUNCTIONS/////
	
	///States///
	protected virtual void Waiting(){ /*Do nothing...*/ }
    protected virtual void Activating()
    {
        activationCount += Time.deltaTime;
        if (activationCount >= timeToActive)
        {
            activationCount = 0;
            Activate();
        }
    }
    protected virtual void Reloading()
    {
        if(!frozen)
        {
            reloadingCount += Time.deltaTime;
            if (reloadingCount >= activationCD)
            {
                Reloaded();
                reloadingCount = 0;
            }
        }
    }
	
	///Waiting -> Activating///
    protected virtual void Activation() //Players triggers on trap zone
    {
        if(actualState == States.Waiting)
        {
            actualState = States.Activating;
            ChangeAnim();
        }
    }

    protected virtual void Activate() //Trap attacks
    {
        StartReload();
        StartCoroutine(ActivateZone());
    }

    protected virtual IEnumerator ActivateZone()
    {
        yield return new WaitForSeconds(timeAnimBefore);
        damageZone.SetActive(true);
        if (particle != null) particle.Play();
        StartCoroutine(DisableDamageZone());
    }

    protected IEnumerator DisableDamageZone()
    {
        yield return new WaitForSeconds(0.1f);
        damageZone.SetActive(false);
    }
	
	///Activating -> Reloading///
    protected void StartReload() //Trap starts Reloading
    {
        actualState = States.Reloading;
        ChangeAnim();
    }
	
	///Reloading -> Waiting///
    protected virtual void Reloaded() //Trap Reloaded
    {
        actualState = States.Waiting;
        ChangeAnim();
    }

    protected virtual void ChangeAnim()
    {
        switch (actualState)
        {
            case States.Waiting:
                outl.OutlineColor = Color.white;
                state = 0;
                break;
            case States.Activating:
                outl.OutlineColor =Color.red;
                state = 1;
                break;
            case States.Reloading:
                outl.OutlineColor = new Color(0, 0, 0, 0);
                state = 2;
                break;
        }

        if (anim != null) anim.SetInteger("State", state);
    }
}

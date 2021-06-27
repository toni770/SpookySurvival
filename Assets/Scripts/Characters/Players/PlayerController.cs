using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Base script for players
//Deals the movement, speed and rotation
//Has a life and dies when reach 0
//Controls all interactions (coin, enemies, damage..)

public class PlayerController : MonoBehaviour
{
    ///////////PUBLIC VARS/////////
    [Header("PlayerConfig")]
    public float InitialSpeed = 6f;            // The speed that the player will move at.
    public GlobalVariables.CharactersTypes CharType;
    public int maxLife;
    public float inmunityDuration = 1;
    public float blinkTime = 0.2f;

    public float habilityCD = 3;
    public GameObject shieldEffect;
    public GameObject model;
    ///////////PRIVATE VARS/////////

    protected int actualLife;
    protected float actualSpeed;

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    protected Animator anim;                      // Reference to the animator component.
    protected Rigidbody rb;                       // Reference to the player's rigidbody.
    SkinnedMeshRenderer[] mesh;
   
    float slowReduction = 1;

    protected bool inmunity;
    bool alive = true;

    public Material lockedMaterial;

    bl_Joystick joystick;

    //Counts
    protected float inmunitycount = 0;
    float blinkCount;
    protected float habilityCount = 0;

    bool canHability = false;
    bool shield = false;

    //Rotation
    public float rotSpeed = 250;
    public float damping = 5;

    //////////////UNITY FUNCTIONS///////////////////////

    void Awake()
    {
        Inicialize();
    }
    protected virtual void FixedUpdate()
    {
        Play();
        InmunityAnimation();
        ReLoadPower();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Daño")
        {
            GetDamage(1, other.tag);
        }
        else if (other.tag == "Slow")
        {
            Slow(true);
        }
        else if (other.tag == "Moneda")
        {
            ManagerGame.Instance.IncreaseMoney(other.GetComponent<Coin>().GetValue());
            Destroy(other.gameObject);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Slow")
        {
            Slow(false);
        }
    }

    private void OnCollisionEnter(Collision collision) //Get damage
    {
        if (collision.transform.tag == "Enemigo")
        {
            EnemyController co = collision.gameObject.GetComponent<EnemyController>();
            if (co is SkeletonController)
            {
                SkeletonController sk = co as SkeletonController;
                if (sk.IsReviving())
                {
                    sk.PermaDeath();
                }
                else
                    GetDamage(1, collision.transform.tag);
            }
            else
                GetDamage(1, collision.transform.tag);
        }
    }

    //////////////PUBLIC FUNCTIONS///////////////////////

    public void ActivateShield(bool activate) //Called by Manager(on get PowerUp);
    {
        shieldEffect.SetActive(activate);
        shield = activate;
    }

    public void ChangeVel(float value) //Called by Manager(on get PowerUp);
    {
        actualSpeed += value;
    }

    public void Slow(bool sl) //Called by SlowTrap(on disable)
    {
        if (sl) slowReduction = 2;
        else slowReduction = 1;
    }

    public void Locked()//Called by MenuController(on load character)
    {
        for(int i=0;i<mesh.Length;i++)
        {
            mesh[i].material = lockedMaterial;
        }
    }

    public void UsePower() //Called by GameManager(on button pressed)
    {
        DisablePower();
        DoPower();
    }

    //////////////PRIVATE FUNCTIONS///////////////////////

    protected virtual void DoPower(){ }

    protected void StopInmunity()
    {
        inmunitycount = 0;
        blinkCount = 0;
        inmunity = false;
        EnableMesh(true);
    }
    protected virtual void Inicialize()
    {
		//GetComponents
    
		anim = transform.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        
		mesh = model.transform.GetComponentsInChildren<SkinnedMeshRenderer>();

        if (ManagerGame.Instance != null)
            joystick = ManagerGame.Instance.GetJoystick();

		//Inicialize vars
        actualLife = maxLife;
        actualSpeed = InitialSpeed;
        inmunity = false;
        shield = false;
        alive = true;
    }
	
	//Move and rotate character
    void Play()
    {
        if (ManagerGame.Instance.Playing())
        {
            // Store the input axes.
            float h = 0;
            float v = 0;

            GetDirection(ref h, ref v);

            AnimateMovement(h, v);
            // Move the player around the scene.
            Move(h, v);
            Rotate(h, v);
        }
    }
	
	//////Movement of Player//////////////
	protected virtual void GetDirection(ref float h, ref float v)
    {
        if(joystick != null)
        {
            h = Mathf.Round(joystick.Horizontal); 
            v = Mathf.Round(joystick.Vertical);
        }

    }

    void AnimateMovement(float h, float v)
    {
        anim.SetFloat("Speed", Mathf.Abs(h) + Mathf.Abs(v));
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = (movement.normalized * actualSpeed * Time.deltaTime)/slowReduction;

        // Move the player to it's current position plus the movement.
        //rb.MovePosition(transform.position + movement);
        transform.Translate(movement,Space.World);
    }

    protected virtual void Rotate(float h, float v)
    {
        float heading = Mathf.Atan2(h, v);
        if(h!=0 || v!=0)
        {
            //transform.rotation = Quaternion.Euler(0, heading * Mathf.Rad2Deg, 0);
            var desiredRotQ = Quaternion.Euler(0, heading * Mathf.Rad2Deg, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * damping);
        }
    }

	////////Inmunity////////////
	void InmunityAnimation() //Blinks when inmunity
    {
        if(inmunity)
        {
            inmunitycount += Time.deltaTime;
            blinkCount += Time.deltaTime;
            if(blinkCount>blinkTime)
            {
                blinkCount = 0;
                StartCoroutine(Blink());
            }
            if(inmunitycount > inmunityDuration)
            {
                StopInmunity();
            }
        }
    }
	
	IEnumerator Blink()
    {
        EnableMesh(false);

        yield return new WaitForSeconds(0.05f);

        EnableMesh(true);
    }
	
    protected virtual void GetInmunity()
    {
        inmunity = true;
    }
	
    void ReLoadPower()
    {
        if (!canHability)
        {
            habilityCount += Time.deltaTime;
            if (habilityCount >= habilityCD)
            {
                habilityCount = 0;
                EnablePower();
            }
        }
    }


    void EnableMesh(bool enable)
    {
        for(int i=0;i<mesh.Length;i++)
        {
            mesh[i].enabled = enable;
        }
    }
    ////////Damage and death////////////
    protected void GetDamage(int damage, string origin)
    {
        if(!inmunity)
        {
            if(!shield)
            {
                ReduceLife(damage, origin);
            }
            else
            {
                ActivateShield(false);
                GetInmunity();
            }

        }
    }
	
	protected virtual void ReduceLife(int damage, string origin)
    {
        actualLife-=damage;
        if (actualLife <= 0)
        {
            Death();
        }
    }
	
	protected void Death()
    {
        if(alive)
        { 
            alive = false;
            anim.SetTrigger("Die");
            ManagerGame.Instance.EnableHability(false);
            ManagerGame.Instance.EndOfGame();
        }

    }   


    protected virtual void DisablePower()
    {
        canHability = false;
        ManagerGame.Instance.EnableHability(false);
    }

    protected virtual void EnablePower()
    {
        canHability = true;
        ManagerGame.Instance.EnableHability(true);
    }
}

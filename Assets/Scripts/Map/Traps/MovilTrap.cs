using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Trap that moves to the player when activating
public class MovilTrap : Traps
{
	///////////PUBLIC VARS/////////
	[Header("MovilTrap Config")]
    public float speed = 2;

	///////////PRIVATE VARS/////////
    Transform target;
    Rigidbody rb;
    Vector3 dir;
	
	///////////FUNCTIONS/////////
    protected override  void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }
    protected override void Activating()
    {
        base.Activating();
        if(target != null)
        {
            /*
                dir = target.position - transform.position;
                dir = dir.normalized;
            */
            //rb.MovePosition(transform.position + (dir*Time.deltaTime * speed));
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            target = other.transform;
            Activation();
        }
    }

}

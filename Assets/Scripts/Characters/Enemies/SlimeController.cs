using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Split in little Slimes when dies.
public class SlimeController : EnemyController
{
	///////////PUBLIC VARS/////////
    [Header("Slime Properties")]
    public int numOfDiv = 1;
    public int numOfChild = 4;
    public GameObject slime;
    public float xMin = -1, xMax = 1, zMin = -1, zMax = 1;
	
	///////////FUNCTIONS/////////
    public void Inicialize(int life)
    {
        this.life = life;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Collider>().isTrigger = false;
    }

    protected override void DestroyEnemy()
    {
        if(numOfDiv == 0)
        {
            base.DestroyEnemy();
        }
        else
        {
            SpawnChildren();
            Destroy(gameObject);
        }
    }

    void SpawnChildren()
    {
        GameObject ob;
        SlimeController sl;
        EnemyController en;
        float rand1, rand2;
        for(int i=0;i<numOfChild;i++)
        {
            rand1 = Random.Range(xMin, xMax);
            rand2 = Random.Range(zMin, zMax);

            ob = Instantiate(slime, new Vector3(transform.position.x + rand1, transform.position.y,
                transform.position.z + rand2), transform.rotation);

            sl = ob.GetComponent<SlimeController>();
            en = sl as EnemyController;
			
            sl.Inicialize(numOfDiv - 1);
            ManagerGame.Instance.NewEnemy(en);
        }
    }
}

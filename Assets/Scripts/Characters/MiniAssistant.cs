using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniAssistant : MonoBehaviour
{
   
    public float lifeTime = 0.5f;
    public GameObject damageZone;

    bool enemyTarget = false;

    UndertakerScript parent;

    Outline outl;
    Rigidbody rb;
    Animator anim;

    float lifeCount = 0;

    // Start is called before the first frame update
    private void Start()
    {
        rb =  GetComponent<Rigidbody>();
        outl = transform.GetComponentInChildren<Outline>();
        anim = transform.GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyTarget)
        {
            lifeCount += Time.deltaTime;
            if (lifeCount > lifeTime)
            {
                lifeCount = 0;
                enemyTarget = false;
                Explosion();
            }
        }
    }

    public void SetParent(UndertakerScript u)
    {
        parent = u;
    }

    public void Walk(bool w)
    {
        anim.SetBool("walk", w);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemigo")
        {
            enemyTarget = true;
            outl.OutlineColor = Color.red;
        }
    }

    void Explosion()
    {

        anim.SetTrigger("attack01");
        StartCoroutine(DestroyObject());
    }

    
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.2f);
        damageZone.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        parent.AssistantDied(this);
        Destroy(gameObject);
    }


}

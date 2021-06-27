using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullet that damages player
public class Bullet : MonoBehaviour
{
    [Header("Bullet Config")]
    public float lifetime = 20;
    public float speed = 10;

    Vector3 direction = Vector3.zero;
    float count = 0;

    public void Inicialize(Vector3 dir)
    {
        direction = dir;
        count = 0;
    }
    // Update is called once per frame
    void Update()
    {
        Life();
        transform.Translate(direction * Time.deltaTime * speed);
    }


    void Life()
    {
        count += Time.deltaTime;
        if (count > lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Enemigo")
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float attractSpeed = 2;
    public float radius = 3;
    public float lifeTime = 2;
    List<Transform> enemies;

    float lifeCount = 0;

    private void Start()
    {
        enemies = new List<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        Atraer();
        lifeCount+=Time.deltaTime;

        if(lifeCount>lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemigo")
        {
            enemies.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemigo")
        {
            enemies.Remove(other.transform);
        }
    }

    void Atraer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            // do whathever you need here to determine if an object is a coin
            // Here I assume that all the coins will be tagged as coin
            if (hitColliders[i].tag == "Enemigo")
            {
                Transform enemy = hitColliders[i].transform;
                enemy.position = Vector3.MoveTowards(enemy.position, transform.position, attractSpeed * Time.deltaTime);
            }
        }
    }

}

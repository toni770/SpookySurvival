using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public float lifeTime = 20;
    float lifeCount = 0;

    // Update is called once per frame
    void Update()
    {
        lifeCount += Time.deltaTime;
        if(lifeCount>= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    float lifeTime = 3;

    float lifeCount = 0;


    // Update is called once per frame
    void Update()
    {
        lifeCount += Time.deltaTime;
        if(lifeCount>lifeTime)
        {
            lifeTime = 0;
            Destroy(gameObject);
        }
    }

    public void SetLifeTime(float life)
    {
        lifeTime = life;
    }
}

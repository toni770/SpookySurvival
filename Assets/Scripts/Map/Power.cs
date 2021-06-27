using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If collides with the Player, give him a power and destroy itself.
public class Power : MonoBehaviour
{
    public GlobalVariables.Powers powerType;
    public float lifetime = 10;

    float lifeCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ManagerGame.Instance.GetPower(powerType, true);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        lifeCount += Time.deltaTime;
        if(lifeCount> lifetime)
        {
            Destroy(gameObject);
        }
    }
}

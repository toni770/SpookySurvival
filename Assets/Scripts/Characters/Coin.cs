using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Just stores a value so could be different tipes of coins
public class Coin : MonoBehaviour
{
	///////////PUBLIC VARS/////////
    public int value;
    public float lifeTime = 10;
	
	///////////PRIVATE VARS/////////
    float lifeCount = 0;

	//////////FUNCTIONS/////////
    private void Start()
    {
        lifeCount = 0;
    }

    private void Update()
    {
        lifeCount += Time.deltaTime;
        if(lifeCount>=lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public int GetValue() //Called by: Player(on trigger)
    {
        return value;
    }


}

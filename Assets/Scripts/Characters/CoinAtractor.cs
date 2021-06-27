using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If Player intersect the area, coin goes to him, so he doesn't need to touch the coin.
public class CoinAtractor : MonoBehaviour
{
    public float AtractionVelocity = 3;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
           transform.parent.position = Vector3.MoveTowards(transform.parent.position, other.transform.position, AtractionVelocity * Time.deltaTime);
        }
    }
}

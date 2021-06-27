using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enables to transport the player to his sibling portal. Then, get disabled for a while.
public class Portal : MonoBehaviour
{

    public Transform siblingPosition;

    public float cdActivation = 3;
    float countActivation = 0;
    bool loading = false;

    public GameObject door;

    Portal siblingPortal;


    void Awake()
    {
        siblingPortal = siblingPosition.gameObject.GetComponent<Portal>();
        Inicialize();
    }


    void Update()
    {
        if (loading)
        {
            countActivation += Time.deltaTime;
            if (countActivation >= cdActivation)
            {
                Avaliable();
                countActivation = 0;
            }
        }
    }

    void Inicialize()
    {
        loading = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!loading)
        {
            Active(other.transform);
        }
    }

    void Active(Transform player)
    {
        Disabled();
        siblingPortal.Disabled();
        player.position = siblingPosition.position;
    }

    public void Disabled()
    {
        loading = true;
        door.SetActive(true);
    }

    void Avaliable()
    {
        loading = false;
        door.SetActive(false);
    }


}

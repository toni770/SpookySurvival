using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowController : MonoBehaviour
{
    PlayerController player;
    List<EnemyController> enemies;

    private void Awake()
    {
        enemies = new List<EnemyController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<PlayerController>();
        }
        else if (other.tag == "Enemigo")
        {
            enemies.Add(other.GetComponent<EnemyController>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player = null;
        }
        else if (other.tag == "Enemigo")
        {
            enemies.Remove(other.GetComponent<EnemyController>());
        }
    }
    private void OnDisable()
    {
        WarnCharacters();
    }

    void WarnCharacters()
    {
        if (player != null)
        {
            player.Slow(false);
            player = null;
        }

        for (int i = 0; i < enemies.Count; i++)
        {

        }
    }
}

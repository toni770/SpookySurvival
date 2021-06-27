using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndertakerScript : PlayerController
{
    public GameObject assistant;
    public float assistantRadius = 5;
    public int maxAssistants = 10;

    GameObject asGroupObject;
    int assistantNum = 0;
    GameObject assistantGroup;

    List<MiniAssistant> listAssistants;

    private void Start()
    {
        listAssistants = new List<MiniAssistant>();

        ManagerGame.Instance.ChangePlantText();
        asGroupObject = new GameObject();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (asGroupObject != null)
            asGroupObject.transform.position = transform.position;
    }

    protected override void GetDirection(ref float h, ref float v)
    {
        base.GetDirection(ref h,ref v);
        if(h != 0 || v != 0)
        {
            AssistantWalk(true);
        }
        else
        {
            AssistantWalk(false);
        }
    }

    public void AssistantWalk(bool w)
    {
        for(int i=0;i<listAssistants.Count;i++)
        {
            listAssistants[i].Walk(w);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Daño")
        {
            print(other.name);
            if(other.name != "assistant") GetDamage(1, other.tag);
        }
        else if (other.tag == "Slow")
        {
            Slow(true);
        }
        else if (other.tag == "Moneda")
        {
            ManagerGame.Instance.IncreaseMoney(other.GetComponent<Coin>().GetValue());
            Destroy(other.gameObject);
        }
    }

    protected override void DoPower()
    {
      if(ManagerGame.Instance.PlantCount() > 0)
            SpawnAssistants();
    }

    protected override void EnablePower()
    {
        if(assistantNum <= 0)
            base.EnablePower();
    }
    protected override void DisablePower()
    {
        if(ManagerGame.Instance.PlantCount() > 0)
            base.DisablePower();
    }
    void SpawnAssistants()
    {
        assistantNum = ManagerGame.Instance.PlantCount();
        ManagerGame.Instance.DestroyPlants();

        CreateEnemiesAroundPoint(assistantNum, transform.position, assistantRadius);
    }

    public void AssistantDied(MiniAssistant go)
    {
        assistantNum--;
        listAssistants.Remove(go);
        if (assistantNum <= 0 && habilityCount >= habilityCD)
        {
            Destroy(asGroupObject);
            asGroupObject = null;
            EnablePower();
        }
    }

    public int GetAssistantNum() //Called by Manaeger(on check if can spawn plant)
    {
        return assistantNum;
    }

    public int GetMaxAssistants() //Called by Manaeger(on check if can spawn plant)
    {
        return maxAssistants;
    }
    void CreateEnemiesAroundPoint(int num, Vector3 point, float radius)
    {

        for (int i = 0; i < num; i++)
        {

            /* Distance around the circle */
            var radians = 2 * Mathf.PI / num * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            var enemy = Instantiate(assistant, spawnPos, Quaternion.identity) as GameObject;
            enemy.transform.parent = asGroupObject.transform;
            enemy.GetComponent<MiniAssistant>().SetParent(this);
            listAssistants.Add(enemy.GetComponent<MiniAssistant>());

            /* Rotate the enemy to face towards player */
            enemy.transform.LookAt(2*spawnPos - transform.position);

            /* Adjust height */
            enemy.transform.Translate(new Vector3(0, enemy.transform.localScale.y / 2, 0));
        }
    }

}

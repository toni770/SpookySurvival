using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Base cript for chests. Gives a some reward when open depends on the lvl. It opens again after a while.

public class Chest : MonoBehaviour
{
	///////////PUBLIC VARS/////////
	[Header("ChestConfig")]
	public GameObject chest_close, chest_open;
    public float closeDuration = 10;
    public Text puntTxt;
    public ParticleSystem part;

    [Tooltip("Num of score minimum to put up lvl. Pos 0 = lvl 0.")]
    public int[] numOfPunt; //Num Of Kills needed to level up.
   
   [Tooltip("Color outline of each lvl. Pos 0 = lvl 0.")]
    public Color[] lvlColors; //Color Outline 
	///////////PRIVATE VARS/////////
    bool opened;
    protected int lvl = 0;
    float closeCount = 0;
	
	Outline outl;
	BoxCollider bc;

    int punt;

    /////UNITY FUNCTIONS/////
    protected virtual void Start()
    {
        outl = GetComponentInChildren<Outline>();
        bc = GetComponent<BoxCollider>();
        opened = false;
        ResetLvl();
    }
	
    private void Update()
    {
        closeCount += Time.deltaTime;
        if(closeCount>closeDuration && opened)
        {
            closeCount = 0;
            Close();
        }
    }
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Open();
		}
	}
	
	/////PUBLIC FUNCTIONS////////
    public void IncrementPunt() //Called by: Manager(On Kill Enemy)
    {
        if (!opened && (lvl < numOfPunt.Length))
        {
            punt++;
            puntTxt.text = punt.ToString();
            if (punt >= numOfPunt[lvl])
            {
                LevelUp();
            }
        }
    }

	/////PRIVATE FUNCTIONS////////
    void Open()
    {
        chest_close.SetActive(false);
        chest_open.SetActive(true);
        bc.enabled = false;
        opened = true;
        puntTxt.text = "";
        closeCount = 0;

        GiveReward();
    }

    void Close()
    {
        chest_close.SetActive(true);
        chest_open.SetActive(false);
        bc.enabled = true;
        opened = false;

        ResetLvl();
    }

    void ResetLvl()
    {
        lvl = 0;
        outl.OutlineColor =  lvlColors[lvl];
        punt = 0;
        puntTxt.text = punt.ToString();
        if (punt >= numOfPunt[lvl])
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        lvl++;
        outl.OutlineColor = lvlColors[lvl];
    }

    protected virtual void GiveReward() {
        if (lvl == 0) { part.Play(); }
    }

}

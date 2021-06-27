using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Manages all the User Interface of the game
    //Shows and hide the diferent screens
    //Updates variables on screen
    //Contains button's code

public class UIManager : MonoBehaviour
{
	///////////PUBLIC VARS/////////
    public Text comboTxt;
    public Text moneyTxt;
    public Text score;
    public Text money;
    public Text waveTxt;
    public Text bestWave;
    public Text pauseMoney;
    public Text pausePunt;
    public Text totalCoins;
    public GameObject endScene;
    public GameObject gameScene;
    public GameObject pauseScene;
    public Button habilityButton;
    public Text habilityButtonText;
    //Gollem
    public Slider GollemEnergy;
    public Image fillEnergy;
    //King 
    public GameObject KingImage;
    public GameObject UndertakerImage;

    public Sprite[] habilityImages;
    public Sprite warriorSecondHability;

    [Header("GolemConfig")]
    public Color energyLow;
    public Color energyMedium;
    public Color energyHigh;

    GlobalVariables.CharactersTypes classTried;

    //Singleton
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UIManager is NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
	
    public void Config(GlobalVariables.CharactersTypes type) //Called by : GameManager(on load game)
    {
        classTried = type;
        switch (type)
        {
            case GlobalVariables.CharactersTypes.Gollem:
                ShowEnergy();
                break;
            case GlobalVariables.CharactersTypes.King:
                KingImage.SetActive(true);
                break;
            case GlobalVariables.CharactersTypes.Undertaker:
                UndertakerImage.SetActive(true);
                break;
        }

        habilityButton.image.sprite = habilityImages[(int)type];
    }

    void Pause(bool pause)
    {
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    //Golem CONFIG
    void ShowEnergy()
    {
        GollemEnergy.gameObject.SetActive(true);
    }
    public void UpdateEnergy(float energy)
    {
        GollemEnergy.value = energy;
        if(energy < 50)
        {
            fillEnergy.color = energyLow;
        }
        else if(energy >50 && energy < 100)
        {
            fillEnergy.color = energyMedium;
        }
        else
        {
            fillEnergy.color = energyHigh;
        }
    }

    //Warrior Config
    public void ChangeImage()
    {
        if (habilityButton.image.sprite == habilityImages[(int)classTried])
        {
            habilityButton.image.sprite = warriorSecondHability;
        }
        else
        {
            habilityButton.image.sprite = habilityImages[(int)classTried];

        }
    }


	//////// CHANGE TEXT ////// Calleds by Manager
    
	public void UpdateMoney(string text)
    {
        moneyTxt.text = text;
    }

    public void UpdateWave(string text)
    {
        waveTxt.text = text;
    }

    public void UpdateTotalMoney(string text)
    {
        totalCoins.text = text;
    }

    public void UpdateBestWave(string text)
    {
        bestWave.text = text;
    }


    public void UpdateCombo(string text)
    {
        if (int.Parse(text) == 0)
            comboTxt.text = "";
        else
            comboTxt.text = 'x' + text;  
    }

    public void EnableHability(bool enable)
    {
        habilityButton.interactable = enable;
    }

    public void ChangeButtonText(string text)
    {
        habilityButtonText.text = text;
    }

    //////// BUTTONS /////////

    //PauseMenu
    public void SetPause()
    {
        pauseMoney.text = moneyTxt.text;
        pausePunt.text = waveTxt.text;
        Pause(true);
        gameScene.SetActive(false);
        pauseScene.SetActive(true);
    }
    public void ResumeGame()
    {
        Pause(false);
        gameScene.SetActive(true);
        pauseScene.SetActive(false);
    }
    ///EndGameMenu
    public void RetryGame()
    {
        Pause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Pause(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void EndOfGame()
    {
        gameScene.SetActive(false);


        score.text = waveTxt.text;
        money.text = moneyTxt.text;
        endScene.SetActive(true);
    }

    public void Hability()
    {
        ManagerGame.Instance.UsePower();
        if(classTried == GlobalVariables.CharactersTypes.Warrior)
        {
            ChangeImage();
        }
    }



}

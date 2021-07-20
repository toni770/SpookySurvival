using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Controls the main menu of the game
public class MenuController : MonoBehaviour
{
    [Header("Menu Config")]
    ///////PUBLIC VARS//////////
    public GameObject camStart;
    public GameObject camSelect;
    public GameObject tapToPlay;
    public GameObject SelectMenu;
    public GameObject infoPersonajes;
    public InfoCharacters info;

    public Text coinTxt;
    public Button btnStart;
    public Button btnUnlock;
    public Button btnUpgrade;
    public Text priceTxt;
    public Text priceUpgradeTxt;
    public Text lvlTxt;

    [Header("Habilities")]
    public GameObject[] habilitiesMenu;

    [Header("Characters")]
    public GameObject[] chars;
    public Transform spawnChar;
    public int[] minimumCoins;
    public int[] coinsUpgrade;


    ///////PRIVATE VARS//////////
    Animator camStartAnimator;
    int index;
    GameObject playerSpawned;
    GameObject habilitySpawned;

    ///////UNITY FUNCTIONS//////////
    void Awake()
    {
        //SaveSystem.SaveData();
        if (SaveSystem.SelectCharacter)
        {
            SelectCharacterMode();
        }
        //SaveSystem.SaveData();
        LoadData();
        coinTxt.text = SaveSystem.coins.ToString();

        camStartAnimator = camStart.GetComponent<Animator>();
        index = PlayerPrefs.GetInt("PlayerSelected", 0);
        Spawn();
    }

    ///////PUBLIC FUNCTIONS//////////
    public void SelectChar()
    {
        tapToPlay.SetActive(false);
        camStartAnimator.SetBool("Select", true);
    }

    public void ShowSelectButtons()
    {
        SelectMenu.SetActive(true);
        infoPersonajes.SetActive(true);
    }

    public void Left()
    {
        index--;
        if(index < 0)
        {
            index = chars.Length - 1;
        }
        Spawn();
    }
    public void Right()
    {
        index++;
        if(index > chars.Length - 1)
        {
            index = 0;
        }
        Spawn();
    }
    public void Play()
    {
        PlayerPrefs.SetInt("PlayerSelected", index);
        PlayerPrefs.SetInt("CharacterLvl", SaveSystem.characterlvl[index]);
        SceneManager.LoadScene("Joc");
    }

    public void Unlock()
    {
        SaveSystem.characterUnlock[index] = true;
        SaveSystem.coins -= minimumCoins[index];
        coinTxt.text = SaveSystem.coins.ToString();

        ///Animacio en un futur
        Spawn();

        SaveSystem.SaveData();
    }

    public void Upgrade()
    {
        if(SaveSystem.characterlvl[index] < 3)
            SaveSystem.characterlvl[index]++;

        SaveSystem.coins -= coinsUpgrade[index];
        coinTxt.text = SaveSystem.coins.ToString();

        lvlTxt.text = "Lvl." + SaveSystem.characterlvl[index].ToString();

        btnUpgrade.gameObject.SetActive(SaveSystem.characterlvl[index] < 3);
        priceUpgradeTxt.gameObject.SetActive(SaveSystem.characterlvl[index] < 3);

        priceUpgradeTxt.text = (coinsUpgrade[index] * SaveSystem.characterlvl[index]).ToString();
        btnUpgrade.interactable = SaveSystem.coins >= coinsUpgrade[index];

        SaveSystem.SaveData();
    }

    ///////PRIVATE FUNCTIONS//////////
    void Spawn()
    {
        if (playerSpawned != null) Destroy(playerSpawned);
        if (habilitySpawned != null) Destroy(habilitySpawned);

        playerSpawned = Instantiate(chars[index], spawnChar.position, spawnChar.rotation);
        playerSpawned.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        Destroy(playerSpawned.GetComponent<PlayerController>());

        lvlTxt.text = "Lvl." + SaveSystem.characterlvl[index].ToString();

        playerSpawned.AddComponent<MoveCharacterSelect>();

        if (!SaveSystem.characterUnlock[index]) playerSpawned.GetComponent<PlayerController>().Locked();

        DisableStart(!SaveSystem.characterUnlock[index]);
        EnableBuy(!SaveSystem.characterUnlock[index]);

        info.SetInfo(index);
    }

    void DisableStart(bool dis)
    {
        btnStart.interactable = !dis;
    }

    void EnableBuy(bool ena)
    {
        btnUpgrade.gameObject.SetActive(SaveSystem.characterlvl[index] < 3 &&!ena);
        priceUpgradeTxt.gameObject.SetActive(SaveSystem.characterlvl[index] < 3 && !ena);

        btnUnlock.gameObject.SetActive(ena);
        priceTxt.gameObject.SetActive(ena);

        if (ena)
        {
            priceTxt.text = minimumCoins[index].ToString();
            btnUnlock.interactable = SaveSystem.coins >= minimumCoins[index];
        }
        else
        {
            habilitySpawned = Instantiate(habilitiesMenu[index], SelectMenu.transform);

            priceUpgradeTxt.text = (coinsUpgrade[index] * SaveSystem.characterlvl[index]).ToString();
            btnUpgrade.interactable = SaveSystem.coins >= coinsUpgrade[index];
        }
    }

    void LoadData()
    {
        SaveSystem.LoadData();
    }

    void SelectCharacterMode()
    {
        camStart.SetActive(false);
        camSelect.SetActive(true);

        tapToPlay.SetActive(false);
        ShowSelectButtons();
    }
}

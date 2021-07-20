using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static bool[] characterUnlock = new bool[System.Enum.GetNames(typeof(GlobalVariables.CharactersTypes)).Length];
    public static int[] characterlvl = new int[System.Enum.GetNames(typeof(GlobalVariables.CharactersTypes)).Length];
    public static int coins = 0;
    public static int maxWave = 0;
    public static bool SelectCharacter = false;

    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(characterUnlock,characterlvl, coins,maxWave);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public static void LoadData()
    {
        string path = Application.persistentDataPath + "/player.data";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            characterUnlock = data.characterUnlock;
            characterlvl = data.characterlvl;
            coins = data.coins;
            maxWave = data.maxWave;
        }
        else
        {
            Debug.Log("Save file not found in " + path);

            for (int i = 0; i < characterUnlock.Length; i++)
            {
                characterUnlock[i] = false;
                characterlvl[i] = 1;
            }

            characterUnlock[0] = true;
        }
    }
}

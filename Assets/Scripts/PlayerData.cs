using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool[] characterUnlock;
    public int[] characterlvl;
    public int coins;
    public int maxWave;

    public PlayerData(bool[] _characterUnlock, int[] _characterlvl, int _coins = 0, int _maxWave = 0)
    {
        coins = _coins;
        maxWave = _maxWave;
        characterUnlock = _characterUnlock;
        characterlvl = _characterlvl;

        for (int i = 0; i < characterlvl.Length; i++) characterlvl[i] = 1;
        for (int i = 0; i < characterUnlock.Length; i++) characterUnlock[i] = false;

        characterUnlock[0] = true;
    }

}

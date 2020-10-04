using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class gameData 
{
    public int Level;
    public bool[] completedLevels;
    public int Volume;

    public gameData(int l, bool[] c, int v)
    {
        Level = l;
        completedLevels = new bool[c.Length];
        for (int i = 0; i < c.Length; i++)
        {
            completedLevels[i] = c[i];
        }
        Volume = v;
    }
}
